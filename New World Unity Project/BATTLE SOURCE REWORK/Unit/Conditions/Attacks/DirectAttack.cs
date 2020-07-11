﻿using NewWorld.Battlefield.Unit.Actions.UnitUpdates;
using NewWorld.Battlefield.Unit.Durability;
using NewWorld.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Unit.Conditions.Attacks {

    public class DirectAttack : AttackCondition {

        // Static.

        private static readonly int attackSpeedAnimatorHash = Animator.StringToHash("AttackSpeed");


        // Properties.

        public override bool CanBeCancelled => !attacked;


        // Fields.

        private bool attacked;
        private float damageTime;
        private float finishTime;


        // Constructors.

        public DirectAttack(
            UnitController target,
            float attackPower = 1, float attackSpeed = 1, float attackTime = 0.5f
        ) : base(target, attackPower, attackSpeed, attackTime) {}


        // Life cycle.

        protected override IEnumerable<GameAction> OnEnter() {
            attacked = false;
            if (Target == null) {
                attacked = true;
                finishTime = Time.time;
                return Enumerables.GetNothing<GameAction>();
            }

            float currentTime = Time.time;
            float attackPeriod = 1 / AttackSpeed;
            damageTime = currentTime + AttackTime * attackPeriod;
            finishTime = currentTime + attackPeriod;

            var setRotation = new RotationUpdate(Parent, Quaternion.LookRotation(Target.Position - Parent.Position));
            var animatorParameterUpdate = new AnimatorParameterUpdate<float>(Owner, attackSpeedAnimatorHash, AttackSpeed);
            return new GameAction[] { setRotation, animatorParameterUpdate };
        }

        protected override IEnumerable<GameAction> OnUpdate(out bool completed) {
            completed = false;
            var actions = Enumerables.GetNothing<GameAction>();

            if (!attacked) {
                if (Target == null) {
                    completed = true;
                } else if (Time.time >= damageTime) {
                    var action = new DamageTaking(Target, AttackPower);
                    actions = Enumerables.GetSingle(action);
                    attacked = true;
                }
            }

            if (attacked) {
                if (Time.time >= finishTime) {
                    completed = true;
                }
            }

            return actions;
        }

        protected override IEnumerable<GameAction> OnFinish(StopType stopType) {
            var action = new AnimatorParameterUpdate<float>(Owner, attackSpeedAnimatorHash, 0);
            return Enumerables.GetSingle(action);
        }


    }

}
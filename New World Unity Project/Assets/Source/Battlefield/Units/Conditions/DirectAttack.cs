using NewWorld;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal;
using NewWorld.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions {

    public class DirectAttack : Condition {

        // Static.

        private static readonly int attackSpeedAnimatorHash = Animator.StringToHash("AttackSpeed");


        // Properties.

        public override bool CanBeCancelled => !attacked;


        // Fields.

        // Parameters.
        private readonly UnitController target;
        private readonly float attackPower = 1;
        private readonly float attackSpeed = 1;

        // Condition.
        private bool attacked;
        private float damageTime;
        private float finishTime;


        // Constructors.

        public DirectAttack(UnitController owner, UnitController target, float attackPower = 1, float attackSpeed = 1) : base(owner) {
            this.target = target;
            this.attackPower = Mathf.Max(0, attackPower);
            this.attackSpeed = Mathf.Max(0, attackSpeed);
        }


        // Life cycle.

        protected override IEnumerable<GameAction> OnEnter() {
            attacked = false;
            if (target == null) {
                attacked = true;
                finishTime = Time.time;
                return Enumerables.GetNothing<GameAction>();
            }

            float currentTime = Time.time;
            float attackPeriod = 1 / attackSpeed;
            damageTime = currentTime + attackPeriod / 2;
            finishTime = currentTime + attackPeriod;

            var unitMoving = new UnitMoving(Owner, null, Quaternion.LookRotation(target.Position - Owner.Position));
            var animatorParameterUpdate = new AnimatorParameterUpdate<float>(Owner, attackSpeedAnimatorHash, attackSpeed);
            return new GameAction[] { unitMoving, animatorParameterUpdate };
        }

        protected override IEnumerable<GameAction> OnUpdate(out bool completed) {
            completed = false;
            var actions = Enumerables.GetNothing<GameAction>();

            if (!attacked) {
                if (target == null) {
                    completed = true;
                } else if (Time.time >= damageTime) {
                    var action = new DamageCausing(target, attackPower);
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

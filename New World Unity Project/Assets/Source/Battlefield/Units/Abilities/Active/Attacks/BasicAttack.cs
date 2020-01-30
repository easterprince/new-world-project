using System.Collections.Generic;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Active.Attacks {

    public class BasicAttack : AttackAbility {

        // Static.

        private static readonly int attackSpeedAnimatorHash = Animator.StringToHash("AttackSpeed");


        // Properties.

        public override bool CanBeCancelled => !attacked;


        // Fields.

        // Parameters.
        private const float attackSpeed = 1;
        private const float attackPower = 1;

        // Condition.
        private bool attacked;
        private float damageTime;
        private float finishTime;


        // Constructors.

        public BasicAttack(UnitController owner) : base(owner) {}


        // Life cycle.

        protected override IEnumerable<GameAction> OnStart() {
            attacked = false;
            if (Target == null) {
                attacked = true;
                finishTime = Time.time;
                return Enumerables.GetNothing<GameAction>();
            }

            float currentTime = Time.time;
            float attackPeriod = 1 / attackSpeed;
            damageTime = currentTime + attackPeriod / 2;
            finishTime = currentTime + attackPeriod;

            var animatorParameterUpdate = new AnimatorParameterUpdate<float>(Owner, attackSpeedAnimatorHash, attackSpeed);
            var transformUpdate = new TransformUpdate(Owner, null, Quaternion.LookRotation(Target.Position - Owner.Position));
            return new GameAction[] { transformUpdate, animatorParameterUpdate };
        }

        protected override IEnumerable<GameAction> OnUpdate(out bool completed) {
            completed = false;
            var actions = Enumerables.GetNothing<GameAction>();

            if (!attacked) {
                if (Target == null) {
                    completed = true;
                } else if (Time.time >= damageTime) {
                    var action = new DamageCausing(Target, attackPower);
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

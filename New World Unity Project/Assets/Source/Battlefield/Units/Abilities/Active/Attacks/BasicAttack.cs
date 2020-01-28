using System.Collections.Generic;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Active.Attacks {

    public class BasicAttack : AttackAbility {

        // Static.

        private static readonly int attackSpeedAnimatorHash = Animator.StringToHash("AttackSpeed");


        // Properties.

        public override bool CanBeCancelled => throw new System.NotImplementedException();


        // Fields.

        // Parameters.
        private const float attackSpeed = 1;
        private const float attackPower = 1;

        // Condition.
        private float damageTime;
        private float finishTime;


        // Constructors.

        public BasicAttack(UnitController owner) : base(owner) { }


        // Life cycle.

        protected override IEnumerable<GameAction> OnStart() {
            float currentTime = Time.time;
            float attackPeriod = 1 / attackSpeed;
            damageTime = currentTime + attackPeriod / 2;
            finishTime = currentTime + attackPeriod;

            var action = new AnimatorParameterUpdate<float>(Owner, attackSpeedAnimatorHash, attackSpeed);
            return Enumerables.GetSingle(action);
        }

        protected override IEnumerable<GameAction> OnUpdate(out bool completed) {
            completed = false;
            var actions = Enumerables.GetNothing<GameAction>();

            if (Target == null) {
                completed = true;
            } else if (Time.time >= damageTime) {
                var action = new DamageCausing(Target, attackPower);
                actions = Enumerables.GetSingle(action);
            }
            if (Time.time >= finishTime) {
                completed = true;
            }

            return actions;
        }

        protected override IEnumerable<GameAction> OnFinish(StopType stopType) {
            var action = new AnimatorParameterUpdate<float>(Owner, attackSpeedAnimatorHash, 0);
            return Enumerables.GetSingle(action);
        }


    }

}

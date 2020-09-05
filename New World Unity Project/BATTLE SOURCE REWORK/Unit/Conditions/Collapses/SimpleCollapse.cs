using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Battlefield.UnitSystem;

namespace NewWorld.Battlefield.Unit.Conditions.Collapses {

    public class SimpleCollapse : CollapseCondition {

        // Static.

        private static readonly int collapseAnimatorHash = Animator.StringToHash("Collapse");
        private static readonly int riseAnimatorHash = Animator.StringToHash("Rise");


        // Fields.

        private float vanishingTime;


        // Constructor.

        public SimpleCollapse(float vanishingPeriod) : base(vanishingPeriod) {}


        // Life cycle.

        override protected IEnumerable<GameAction> OnEnter() {
            vanishingTime = Time.time + VanishingPeriod;
            var action = new AnimatorTriggerApplication(Owner, collapseAnimatorHash);
            return Enumerables.GetSingle<GameAction>(action);
        }

        override protected IEnumerable<GameAction> OnUpdate(out bool completed) {
            completed = false;
            if (Time.time >= vanishingTime) {
                var action = new UnitRemoval(Owner);
                return Enumerables.GetSingle<GameAction>(action);
            }
            return Enumerables.GetNothing<GameAction>();
        }

        override protected IEnumerable<GameAction> OnFinish(StopType stopType) {
            var action = new AnimatorTriggerApplication(Owner, riseAnimatorHash);
            return Enumerables.GetSingle<GameAction>(action);
        }


    }

}

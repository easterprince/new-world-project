using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;

namespace NewWorld.Battlefield.Units.Conditions.Collapses {

    public class SimpleCollapse : CollapseCondition {

        // Static.

        private static readonly int collapseAnimatorHash = Animator.StringToHash("Collapse");
        private static readonly int riseAnimatorHash = Animator.StringToHash("Rise");


        // Fields.

        private float vanishingTime;


        // Constructor.

        public SimpleCollapse(UnitController owner, float vanishingPeriod) : base(owner, vanishingPeriod) {}


        // Life cycle.

        override protected IEnumerable<GameAction> OnEnter() {
            vanishingTime = Time.time + VanishingPeriod;
            var action = new ApplyAnimatorTrigger(Owner, collapseAnimatorHash);
            return Enumerables.GetSingle<GameAction>(action);
        }

        override protected IEnumerable<GameAction> OnUpdate(out bool exited) {
            exited = Time.time >= vanishingTime;
            return Enumerables.GetNothing<GameAction>();
        }

        override protected IEnumerable<GameAction> OnFinish(StopType stopType) {
            GameAction action;
            if (stopType == StopType.Completed) {
                action = new RemoveUnit(Owner);
            } else {
                action = new ApplyAnimatorTrigger(Owner, riseAnimatorHash);
            }
            return Enumerables.GetSingle(action);
        }


    }

}

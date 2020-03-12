using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Conditions {
 
    public abstract class UnitCondition : UnitModule<UnitController> {

        // Enumerators.

        public enum StatusType {
            NotEntered,
            Entered,
            Exited
        }

        protected enum StopType {
            Completed,
            Cancelled,
            Forced
        }


        // Fields.

        private StatusType status = StatusType.NotEntered;


        // Properties.

        public StatusType Status => status;
        public virtual bool CanBeCancelled => false;
        public virtual string Description => "Unknown condition";


        // Interaction methods.

        public IEnumerable<GameAction> Enter(ParentPassport<UnitController> parentPassport) {
            ValidatePassport(parentPassport);
            if (status != StatusType.NotEntered) {
                throw new System.InvalidOperationException($"Condition status must be {StatusType.NotEntered}.");
            }
            status = StatusType.Entered;
            return OnEnter();
        }

        public IEnumerable<GameAction> Update(ParentPassport<UnitController> parentPassport) {
            ValidatePassport(parentPassport);
            if (status != StatusType.Entered) {
                throw new System.InvalidOperationException($"Condition status must be {StatusType.Entered}.");
            }
            var actions = OnUpdate(out bool completed);
            if (completed) {
                var otherActions = OnFinish(StopType.Completed);
                status = StatusType.Exited;
                actions = Enumerables.Unite(actions, otherActions);
            }
            return actions;
        }

        public IEnumerable<GameAction> Stop(ParentPassport<UnitController> parentPassport, bool forceStop) {
            ValidatePassport(parentPassport);
            if (status != StatusType.Entered) {
                throw new System.InvalidOperationException($"Condition status must be {StatusType.Entered}.");
            }
            if (!forceStop && !CanBeCancelled) {
                return Enumerables.GetNothing<GameAction>();
            }
            var actions = OnFinish(forceStop ? StopType.Forced : StopType.Cancelled);
            status = StatusType.Exited;
            return actions;
        }


        // Life cycle.

        protected abstract IEnumerable<GameAction> OnEnter();

        protected abstract IEnumerable<GameAction> OnUpdate(out bool completed);

        protected abstract IEnumerable<GameAction> OnFinish(StopType stopType);


    }

}
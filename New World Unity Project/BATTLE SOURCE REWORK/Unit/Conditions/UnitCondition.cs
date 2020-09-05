using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Battlefield.Unit.Core;

namespace NewWorld.Battlefield.Unit.Conditions {

    public abstract class UnitCondition : UnitModuleBase<UnitCondition, UnitCore, UnitConditionPresentation> {

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

        public void Enter() {
            ValidateOwnership();
            if (status != StatusType.NotEntered) {
                throw new System.InvalidOperationException($"Condition status must be {StatusType.NotEntered}.");
            }
            status = StatusType.Entered;
            OnEnter();
        }

        public void Update() {
            ValidateOwnership();
            if (status != StatusType.Entered) {
                throw new System.InvalidOperationException($"Condition status must be {StatusType.Entered}.");
            }
            OnUpdate(out bool completed);
            if (completed) {
                OnFinish(StopType.Completed);
                status = StatusType.Exited;
            }
        }

        public void Stop(bool forceStop) {
            ValidateOwnership();
            if (status != StatusType.Entered) {
                throw new System.InvalidOperationException($"Condition status must be {StatusType.Entered}.");
            }
            if (forceStop || CanBeCancelled) {
                OnFinish(forceStop ? StopType.Forced : StopType.Cancelled);
                status = StatusType.Exited;
            }
        }


        // Life cycle.

        protected abstract void OnEnter();

        protected abstract void OnUpdate(out bool completed);

        protected abstract void OnFinish(StopType stopType);


    }

}
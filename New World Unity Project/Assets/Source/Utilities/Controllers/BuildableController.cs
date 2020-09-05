using NewWorld.Utilities.Events;
using System;
using UnityEngine;

namespace NewWorld.Utilities.Controllers {
    
    public class BuildableController : SteadyController {

        // Fields.

        private bool startedBuilding = false;
        private bool finishedBuilding = false;
        private ControllerEvent onBuilt = new ControllerEvent();


        // Properties.

        public bool StartedBuilding {
            get => startedBuilding;
        }

        public bool FinishedBuilding {
            get => finishedBuilding;
        }


        // Events.

        public void ExecuteWhenBuilt(MonoBehaviour subscriber, Action action) {
            if (finishedBuilding) {
                action.Invoke();
                return;
            }
            onBuilt?.AddAction(subscriber, action);
        }

        public void RemoveAction(MonoBehaviour subscriber, Action action) => onBuilt?.RemoveAction(subscriber, action);

        public void RemoveSubscriber(MonoBehaviour subscriber) => onBuilt?.RemoveSubscriber(subscriber);


        // Life cycle.

        private protected virtual void OnDestroy() {
            if (onBuilt != null) {
                onBuilt.Clear();
                onBuilt = null;
            }
        }


        // Methods.

        private protected void SetStartedBuilding() {
            if (startedBuilding) {
                return;
            }
            startedBuilding = true;
        }

        private protected void SetFinishedBuilding() {
            if (finishedBuilding) {
                return;
            }
            SetStartedBuilding();
            finishedBuilding = true;
            if (onBuilt == null) {
                return;
            }
            onBuilt.Invoke();
            onBuilt.Clear();
            onBuilt = null;
        }

        private protected void ValidateNotStartedBuilding() {
            if (startedBuilding) {
                throw new InvalidOperationException("Operation is forbidden after script started building.");
            }
        }

        private protected void ValidateFinishedBuilding() {
            if (finishedBuilding) {
                throw new InvalidOperationException("Operation is forbidden before script finished building.");
            }
        }



    }

}

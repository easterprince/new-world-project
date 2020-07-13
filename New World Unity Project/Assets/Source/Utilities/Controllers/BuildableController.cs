using NewWorld.Utilities.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities.Controllers {
    
    public class BuildableController : MonoBehaviour {

        // Fields.

        private bool built = false;
        private readonly ControllerEvent onBuilt = new ControllerEvent();


        // Properties.

        public bool Built {
            get => built;
            private protected set {
                if (built == value) {
                    return;
                }
                built = value;
                if (built) {
                    onBuilt.Invoke();
                    onBuilt.Clear();
                }
            }
        }


        // Events.

        public void ExecuteWhenBuilt(MonoBehaviour subscriber, Action action) {
            if (built) {
                action.Invoke();
                return;
            }
            onBuilt.AddAction(subscriber, action);
        }

        public void RemoveAction(MonoBehaviour subscriber, Action action) => onBuilt.RemoveAction(subscriber, action);

        public void RemoveSubscriber(MonoBehaviour subscriber) => onBuilt.RemoveSubscriber(subscriber);


        // Life cycle.

        private protected virtual void OnDestroy() {
            onBuilt.Clear();
        }


    }

}

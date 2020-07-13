using System;
using UnityEngine;

namespace NewWorld.Utilities.Events {
    
    public class ControllerEvent : SourcedEventBase<MonoBehaviour, Action> {
    
        // Methods.

        public void Invoke() {
            foreach (var action in GetActions()) {
                action.Invoke();
            }
        }

    
    }


}

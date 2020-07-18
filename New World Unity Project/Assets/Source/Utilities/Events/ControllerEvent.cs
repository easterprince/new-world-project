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

    public class ControllerEvent<TArgument> : SourcedEventBase<MonoBehaviour, Action<TArgument>> {

        // Methods.

        public void Invoke(TArgument argument) {
            foreach (var action in GetActions()) {
                action.Invoke(argument);
            }
        }


    }

    public class ControllerEvent<TArgument1, TArgument2> : SourcedEventBase<MonoBehaviour, Action<TArgument1, TArgument2>> {

        // Methods.

        public void Invoke(TArgument1 argument1, TArgument2 argument2) {
            foreach (var action in GetActions()) {
                action.Invoke(argument1, argument2);
            }
        }


    }


}

using System;
using System.Collections.Generic;

namespace NewWorld.Utilities.Events {

    public class GameEvent : GameEventBase<Action> {

        // Methods.

        public void Invoke() {
            foreach (var queueAndList in Planned) {
                var actionQueue = queueAndList.Key;
                foreach (var action in queueAndList.Value) {
                    actionQueue.Enqueue(action);
                }
            }
        }


    }

    public class GameEvent<TArgument> : GameEventBase<Action<TArgument>> {

        // Methods.

        public void Invoke(TArgument argument) {
            foreach (var queueAndList in Planned) {
                var actionQueue = queueAndList.Key;
                foreach (var action in queueAndList.Value) {
                    actionQueue.Enqueue(() => action.Invoke(argument));
                }
            }
        }


    }

    public class GameEvent<TArgument1, TArgument2> : GameEventBase<Action<TArgument1, TArgument2>> {

        // Methods.

        public void Invoke(TArgument1 argument1, TArgument2 argument2) {
            foreach (var queueAndList in Planned) {
                var actionQueue = queueAndList.Key;
                foreach (var action in queueAndList.Value) {
                    actionQueue.Enqueue(() => action.Invoke(argument1, argument2));
                }
            }
        }


    }

}

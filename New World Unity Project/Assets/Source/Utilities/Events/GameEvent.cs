using System;
using System.Collections.Generic;

namespace NewWorld.Utilities.Events {

    public class GameEvent : SourcedEventBase<ActionQueue, Action> {

        // Methods.

        public void Invoke() {
            foreach (var queueAndAction in GetSubscribersAndActions()) {
                var actionQueue = queueAndAction.Key;
                actionQueue.Enqueue(queueAndAction.Value);
            }
        }


    }

    public class GameEvent<TArgument> : SourcedEventBase<ActionQueue, Action<TArgument>> {

        // Methods.

        public void Invoke(TArgument argument) {
            foreach (var queueAndAction in GetSubscribersAndActions()) {
                var actionQueue = queueAndAction.Key;
                actionQueue.Enqueue(() => queueAndAction.Value.Invoke(argument));
            }
        }


    }

    public class GameEvent<TArgument1, TArgument2> : SourcedEventBase<ActionQueue, Action<TArgument1, TArgument2>> {

        // Methods.

        public void Invoke(TArgument1 argument1, TArgument2 argument2) {
            foreach (var queueAndAction in GetSubscribersAndActions()) {
                var actionQueue = queueAndAction.Key;
                actionQueue.Enqueue(() => queueAndAction.Value.Invoke(argument1, argument2));
            }
        }


    }

}

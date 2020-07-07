using System;
using UnityEngine.UIElements;

namespace NewWorld.Utilities.Events {

    public abstract partial class GameEventBase<TAction> {

        // Wrapper class.

        public class Wrapper {

            // Fields.

            private readonly GameEventBase<TAction> gameEvent;


            // Constructor.

            public Wrapper(GameEventBase<TAction> gameEvent) {
                this.gameEvent = gameEvent ?? throw new ArgumentNullException(nameof(gameEvent));
            }


            // Methods.

            public void AddAction(ActionQueue receiver, TAction action) => gameEvent.AddAction(receiver, action);
            public void RemoveAction(ActionQueue receiver, TAction action) => gameEvent.RemoveAction(receiver, action);
            public void RemoveReceiver(ActionQueue receiver) => gameEvent.RemoveReceiver(receiver);


        }

    }

}

using System;
using UnityEngine.UIElements;

namespace NewWorld.Utilities.Events {

    public abstract partial class SourcedEventBase<TSubscriber, TAction> {

        // Wrapper class.

        public class EventWrapper : ClassWrapper<SourcedEventBase<TSubscriber, TAction>> {

            // Constructor.

            public EventWrapper(SourcedEventBase<TSubscriber, TAction> gameEvent) : base(gameEvent) {}


            // Methods.

            public void AddAction(TSubscriber subscriber, TAction action) => Wrapped.AddAction(subscriber, action);
            public void RemoveAction(TSubscriber subscriber, TAction action) => Wrapped.RemoveAction(subscriber, action);
            public void RemoveSubscriber(TSubscriber subscriber) => Wrapped.RemoveSubscriber(subscriber);


        }

    }

}

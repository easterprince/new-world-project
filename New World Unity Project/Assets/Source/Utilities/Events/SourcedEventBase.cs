using System;
using System.Collections.Generic;

namespace NewWorld.Utilities.Events {

    public abstract partial class SourcedEventBase<TSubscriber, TAction>
        where TSubscriber : class
        where TAction : Delegate {

        // Fields.

        private readonly Dictionary<TSubscriber, TAction> planned;
        private readonly EventWrapper wrapper;


        // Constructor.

        public SourcedEventBase() {
            planned = new Dictionary<TSubscriber, TAction>();
            wrapper = new EventWrapper(this);
        }


        // Properties.

        public EventWrapper Wrapper => wrapper;


        // Methods.

        public virtual void AddAction(TSubscriber subscriber, TAction addedAction) {
            if (subscriber is null) {
                throw new ArgumentNullException(nameof(subscriber));
            }
            if (addedAction is null) {
                return;
            }

            if (!planned.TryGetValue(subscriber, out var action)) {
                planned[subscriber] = addedAction;
            }
            planned[subscriber] = (TAction) Delegate.Combine(action, addedAction);

        }

        public virtual void RemoveAction(TSubscriber subscriber, TAction removedAction) {
            if (subscriber is null) {
                throw new ArgumentNullException(nameof(subscriber));
            }
            if (removedAction is null) {
                return;
            }

            if (!planned.TryGetValue(subscriber, out var action)) {
                return;
            }
            var result = (TAction) Delegate.Remove(action, removedAction);
            if (result == null) {
                planned.Remove(subscriber);
            }
            planned[subscriber] = result;

        }

        public virtual void RemoveSubscriber(TSubscriber subscriber) {
            if (subscriber is null) {
                throw new ArgumentNullException(nameof(subscriber));
            }

            planned.Remove(subscriber);

        }

        private protected List<TAction> GetActions() {
            return new List<TAction>(planned.Values);
        }

        private protected Dictionary<TSubscriber, TAction> GetSubscribersAndActions() {
            return new Dictionary<TSubscriber, TAction>(planned);
        }

        public void Clear() {
            planned.Clear();
        }


    }

}

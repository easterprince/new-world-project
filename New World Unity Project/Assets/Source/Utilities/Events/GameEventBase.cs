using System.Collections.Generic;
using UnityEngine.UIElements;

namespace NewWorld.Utilities.Events {
    
    public abstract partial class GameEventBase<TAction> {

        // Fields.

        private readonly Dictionary<ActionQueue, List<TAction>> planned;
        private readonly Wrapper wrapper;


        // Constructor.

        public GameEventBase() {
            planned = new Dictionary<ActionQueue, List<TAction>>();
            wrapper = new Wrapper(this);
        }


        // Properties.

        public Wrapper Gate => wrapper;

        private protected Dictionary<ActionQueue, List<TAction>> Planned => planned;


        // Methods.

        public void AddAction(ActionQueue receiver, TAction action) {
            if (!planned.TryGetValue(receiver, out var actionList)) {
                actionList = new List<TAction>();
                planned[receiver] = actionList;
            }
            if (!actionList.Contains(action)) {
                actionList.Add(action);
            }
        }

        public void RemoveAction(ActionQueue receiver, TAction action) {
            if (!planned.TryGetValue(receiver, out var actionList)) {
                return;
            }
            actionList.Remove(action);
            if (actionList.Count == 0) {
                planned.Remove(receiver);
            }
        }

        public void RemoveReceiver(ActionQueue receiver) {
            planned.Remove(receiver);
        }


    }

}

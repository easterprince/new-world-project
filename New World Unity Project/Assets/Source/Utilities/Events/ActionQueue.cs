using System;
using System.Collections.Generic;

namespace NewWorld.Utilities.Events {

    public class ActionQueue : Queue<Action> {

        // Methods.

        public void RunAll() {
            while (Count > 0) {
                var action = Dequeue();
                action.Invoke();
            }
        }


    }

}

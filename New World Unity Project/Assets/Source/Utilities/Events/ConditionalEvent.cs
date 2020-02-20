using UnityEngine;
using UnityEngine.Events;

namespace NewWorld.Utilities.Events {

    public class ConditionalEvent : UnityEvent {

        // Fields.

        private readonly Condition condition;


        // Constructors.

        public ConditionalEvent(Condition condition) : base() {
            this.condition = condition;
        }


        // Properties.

        public bool MayBeInvoked => condition();


        // Methods.

        public bool TryInvoke() {
            if (!MayBeInvoked) {
                return false;
            }
            Invoke();
            return true;
        }


    }

    public class ConditionalEvent<T0> : UnityEvent<T0> {

        // Fields.

        private readonly Condition condition;


        // Constructors.

        public ConditionalEvent(Condition condition) : base() {
            this.condition = condition;
        }


        // Properties.

        public bool MayBeInvoked => condition();


        // Methods.

        public bool TryInvoke(T0 argument0) {
            if (!MayBeInvoked) {
                return false;
            }
            Invoke(argument0);
            return true;
        }


    }

    public class ConditionalEvent<T0, T1> : UnityEvent<T0, T1> {

        // Fields.

        private readonly Condition condition;


        // Constructors.

        public ConditionalEvent(Condition condition) : base() {
            this.condition = condition;
        }


        // Properties.

        public bool MayBeInvoked => condition();


        // Methods.

        public bool TryInvoke(T0 argument0, T1 argument1) {
            if (!MayBeInvoked) {
                return false;
            }
            Invoke(argument0, argument1);
            return true;
        }


    }


}

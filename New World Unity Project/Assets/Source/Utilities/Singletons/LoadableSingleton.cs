using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NewWorld.Utilities.Singletons {
    
    public abstract class LoadableSingleton<T, Description> : SceneSingleton<T>
        where T : LoadableSingleton<T, Description> {

        // Fields.

        private bool loaded;
        private UnityEvent unloadedEvent;
        private UnityEvent reloadedEvent;


        // Properties.

        public bool Loaded {
            get => loaded;
            private protected set {
                if (value == loaded) {
                    return;
                }
                loaded = value;
                if (loaded) {
                    reloadedEvent.Invoke();
                } else {
                    unloadedEvent.Invoke();
                }
            }
        }

        public UnityEvent UnloadedEvent => unloadedEvent;
        public UnityEvent ReloadedEvent => reloadedEvent;


        // Life cycle.

        override private protected void Awake() {
            base.Awake();
            unloadedEvent = new UnityEvent();
            reloadedEvent = new UnityEvent();
        }


        // Public methods.

        public abstract void StartReloading(Description description);


    }

}

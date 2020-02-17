using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NewWorld.Utilities.Singletons {
    
    public abstract class ReloadableSingleton<T, Description> : SceneSingleton<T>
        where T : ReloadableSingleton<T, Description> {

        // Fields.

        private bool loaded = true;
        private UnityEvent unloadedEvent = new UnityEvent();
        private UnityEvent loadedEvent = new UnityEvent();


        // Properties.

        public bool Loaded {
            get => loaded;
            private protected set {
                if (value == loaded) {
                    return;
                }
                loaded = value;
                if (loaded) {
                    loadedEvent.Invoke();
                } else {
                    unloadedEvent.Invoke();
                }
            }
        }

        public UnityEvent UnloadedEvent => unloadedEvent;
        public UnityEvent LoadedEvent => loadedEvent;


        // Public methods.

        public abstract void StartReloading(Description description);


    }

}

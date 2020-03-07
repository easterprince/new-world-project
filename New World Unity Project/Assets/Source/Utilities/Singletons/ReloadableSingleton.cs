using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NewWorld.Utilities.Singletons {
    
    public abstract class ReloadableSingleton<TSelf, TDescription> : SceneSingleton<TSelf>, ILoadable
        where TSelf : ReloadableSingleton<TSelf, TDescription> {

        // Fields.

        private bool loaded = true;
        private readonly UnityEvent unloadedEvent = new UnityEvent();
        private readonly UnityEvent loadedEvent = new UnityEvent();


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


        // Life cycle.

        override private protected void OnDestroy() {
            Loaded = false;
            base.OnDestroy();
        }


        // Public methods.

        public abstract void StartReloading(TDescription description);


    }

}

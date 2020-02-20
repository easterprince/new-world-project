using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities.Singletons {

    public class SceneSingleton<T> : MonoBehaviour
        where T : SceneSingleton<T> {

        // Fields.

        private static T instance;


        // Static.

        public static T Instance {
            get => instance;
        }

        public static void EnsureInstance(object userClass) {
            if (Instance == null) {
                throw new MissingSingletonException<T>(userClass);
            }
        }


        // Life cycle.

        private protected virtual void Awake() {
            if (instance == null) {
                instance = (T) this;
            } else {
                throw new System.Exception($"{typeof(T)} singleton has been already instantiated.");
            }
        }

        private protected virtual void OnDestroy() {
            if (instance == this) {
                instance = null;
            }
        }


    }

}

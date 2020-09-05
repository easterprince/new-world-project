using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities.Singletons {

    public class SceneSingleton<TSelf> : MonoBehaviour
        where TSelf : SceneSingleton<TSelf> {

        // Fields.

        private static TSelf instance;


        // Static.

        public static TSelf Instance {
            get => instance;
        }

        public static void EnsureInstance(object userClass = null) {
            if (Instance == null) {
                throw new MissingSingletonException<TSelf>(userClass);
            }
        }


        // Life cycle.

        private protected virtual void Awake() {
            if (instance == null) {
                instance = (TSelf) this;
            } else {
                throw new System.Exception($"{typeof(TSelf)} singleton has been already instantiated.");
            }
        }

        private protected virtual void OnDestroy() {
            if (instance == this) {
                instance = null;
            }
        }


    }

}

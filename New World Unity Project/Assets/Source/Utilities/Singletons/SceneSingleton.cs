using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities.Singletones {

    public class SceneSingleton<T> : MonoBehaviour
        where T : SceneSingleton<T> {

        // Fields.

        private static T instance;


        // Properties.

        public static T Instance {
            get => instance;
            protected set => instance = value;
        }


        // Life cycle.

        virtual protected void Awake() {}

        virtual protected void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }

    }

}

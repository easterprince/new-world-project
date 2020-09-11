using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities.Singletons {

    public class ControllerSingletonBase<TSelf> : MonoBehaviour
        where TSelf : ControllerSingletonBase<TSelf> {

        // Fields.

        private static List<TSelf> instances = new List<TSelf>();


        // Static.

        public static TSelf Instance => (instances.Count == 0 ? null : instances[0]);

        public static void EnsureInstance(object user = null) {
            if (Instance == null) {
                throw new MissingSingletonException<TSelf>(user);
            }
        }


        // Life cycle.

        private protected virtual void Awake() {
            if (this is TSelf controller) {
                instances.Add(controller);
            }
        }

        private protected virtual void OnDestroy() {
            if (this is TSelf controller) {
                instances.Remove(controller);
            }
        }


    }

}

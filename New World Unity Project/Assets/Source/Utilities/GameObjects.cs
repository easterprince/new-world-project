using UnityEngine;

namespace NewWorld.Utilities {
    
    public static class GameObjects {
    
        public static void SetLayerRecursively(GameObject gameObject, int layer) {
            foreach (Transform child in gameObject.transform) {
                SetLayerRecursively(child.gameObject, layer);
            }
            gameObject.layer = layer;
        }

        public static void ValidateReference<T>(T reference, string referencedName, string objectName = null) {
            if (reference == null) {
                string message;
                if (objectName == null) {
                    message = $"Missing {referencedName} reference.";
                } else {
                    message = $"{objectName} is missing {referencedName} reference.";
                }
                throw new MissingReferenceException(message);
            }
        }

        public static void ValidateComponent<T>(T component, string objectName = null) {
            if (component == null) {
                string message;
                if (objectName == null) {
                    message = $"Missing {typeof(T)} component.";
                } else {
                    message = $"{objectName} is missing {typeof(T)} component.";
                }
                throw new MissingReferenceException(message);
            }
        }

    }

}

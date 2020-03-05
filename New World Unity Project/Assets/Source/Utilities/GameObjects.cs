using UnityEngine;

namespace NewWorld.Utilities {
    
    public static class GameObjects {
    
        public static void SetLayerRecursively(GameObject gameObject, int layer) {
            foreach (Transform child in gameObject.transform) {
                SetLayerRecursively(child.gameObject, layer);
            }
            gameObject.layer = layer;
        }

    }

}

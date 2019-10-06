using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Map {

    public class TileController : MonoBehaviour {

        // Fabric.

        private const string prefabPath = "Prefabs/Tile";
        private static GameObject prefab;

        public static TileController BuildTile(string name = "Tile") {
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject tile = Instantiate(prefab);
            tile.name = name ?? "Tile";
            TileController tileController = tile.GetComponent<TileController>();
            return tileController;
        }


        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        private GameObject block;

#pragma warning restore IDE0044, CS0414, CS0649

        private float height;


        // Properties.

        public float Height => height;


        // Life cycle.

        private void Awake() {}


        // Public methods.

        public void Place(Vector3 surfacePosition) {
            surfacePosition.z = Mathf.Max(surfacePosition.z, 0);

            // Updating object itself.
            height = surfacePosition.z;
            Vector3 centerPosition = surfacePosition;
            centerPosition.z *= 0.5f;
            transform.position = centerPosition;
            transform.localScale = new Vector3(transform.localScale.x, surfacePosition.z, transform.localScale.z);

        }


    }

}

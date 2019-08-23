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
            tile.name = name;
            TileController tileController = tile.GetComponent<TileController>();
            return tileController;
        }


        // Constants.

        private const float ZeroHeightSurfaceBrightness = 0.8f;


        // Fields.

        public GameObject surface;
        public GameObject baseSides;

        private readonly List<GameObject> allSides = new List<GameObject>();


        // Life cycle.

        private void Awake() {
            allSides.Add(baseSides);
        }


        // Public methods.

        public void Place(Vector3 surfaceRealPosition) {
            Vector3 sidesRealPosition = new Vector3(surfaceRealPosition.x, surfaceRealPosition.y, -BattlefieldComposition.TileHidingHeightDifference);
            transform.position = BattlefieldComposition.RealToVisible(sidesRealPosition, 0);
            surface.transform.position = BattlefieldComposition.RealToVisible(surfaceRealPosition, 0, out int spriteOrder);
            surface.GetComponent<SpriteRenderer>().sortingOrder = spriteOrder + (int) BattlefieldComposition.Sublayers.TilesForeground;
            float surfaceBrightness = ZeroHeightSurfaceBrightness + (1 - ZeroHeightSurfaceBrightness) * (surfaceRealPosition.z / MapController.Instance.HeightLimit);
            surface.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 0, surfaceBrightness);
            int index = 0;
            bool lastSides = false;
            do {
                if (sidesRealPosition.z >= surfaceRealPosition.z - BattlefieldComposition.TileHidingHeightDifference) {
                    sidesRealPosition.z = surfaceRealPosition.z - BattlefieldComposition.TileHidingHeightDifference;
                    lastSides = true;
                }
                GameObject sides;
                if (allSides.Count <= index) {
                    sides = Instantiate(baseSides);
                    sides.name = $"Sides ({index})";
                    sides.transform.parent = transform;
                    allSides.Add(sides);
                } else {
                    sides = allSides[index];
                }
                sides.transform.position = BattlefieldComposition.RealToVisible(sidesRealPosition, 0);
                sides.GetComponent<SpriteRenderer>().sortingOrder = spriteOrder +
                    (int) (lastSides ? BattlefieldComposition.Sublayers.TilesBackground : BattlefieldComposition.Sublayers.TilesForeground);
                ++index;
                sidesRealPosition.z += BattlefieldComposition.TileHidingHeightDifference;
            } while (!lastSides);
            for (int i = index; i < allSides.Count; ++i) {
                Destroy(allSides[i]);
            }
            if (index < allSides.Count) {
                allSides.RemoveRange(index, allSides.Count - index);
            }
        }
    }

}

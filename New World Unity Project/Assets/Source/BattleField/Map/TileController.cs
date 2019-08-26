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

        private Vector3 basementRealPosition;
        private int currentDirection;
        private List<GameObject> allSides;


        // Life cycle.

        private void Awake() {
            basementRealPosition = Vector3.zero;
            currentDirection = 0;
            allSides = new List<GameObject> {
                baseSides
            };
        }


        // Public methods.

        public void Place(Vector3 surfaceRealPosition) {

            // Updating object itself.
            basementRealPosition = new Vector3(surfaceRealPosition.x, surfaceRealPosition.y, -BattlefieldComposition.TileHidingHeightDifference);
            transform.position = BattlefieldComposition.RealToVisible(basementRealPosition, currentDirection);

            // Updating surface.
            surface.transform.position = BattlefieldComposition.RealToVisible(surfaceRealPosition, currentDirection, out int spriteOrder);
            surface.GetComponent<SpriteRenderer>().sortingOrder = spriteOrder + (int) BattlefieldComposition.Sublayers.TilesForeground;
            float surfaceBrightness = ZeroHeightSurfaceBrightness + (1 - ZeroHeightSurfaceBrightness) * (surfaceRealPosition.z / MapController.Instance.HeightLimit);
            surface.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 0, surfaceBrightness);

            // Updating sides.
            int index = 0;
            bool lastSides = false;
            Vector3 sidesRealPosition = basementRealPosition;
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
                sides.transform.position = BattlefieldComposition.RealToVisible(sidesRealPosition, currentDirection);
                sides.GetComponent<SpriteRenderer>().sortingOrder = spriteOrder +
                    (int) (lastSides ? BattlefieldComposition.Sublayers.TilesBackground : BattlefieldComposition.Sublayers.TilesForeground);
                ++index;
                sidesRealPosition.z += BattlefieldComposition.TileHidingHeightDifference;
            } while (!lastSides);

            // Removing unnecessary sides.
            while (allSides.Count > index) {
                int last = allSides.Count - 1;
                Destroy(allSides[last]);
                allSides.RemoveAt(last);
            }

        }

        public void Rotate(int newDirection) {

            // Updating object itself.
            currentDirection = newDirection;
            transform.position = BattlefieldComposition.RealToVisible(basementRealPosition, newDirection, out int spriteOrder);

            // Update parts of rile.
            surface.GetComponent<SpriteRenderer>().sortingOrder = spriteOrder + (int) BattlefieldComposition.Sublayers.TilesForeground;
            for (int i = 0; i < allSides.Count; ++i) {
                SpriteRenderer spriteRenderer = allSides[i].GetComponent<SpriteRenderer>();
                if (i == allSides.Count - 1) {
                    spriteRenderer.sortingOrder = spriteOrder + (int) BattlefieldComposition.Sublayers.TilesBackground;
                } else {
                    spriteRenderer.sortingOrder = spriteOrder + (int) BattlefieldComposition.Sublayers.TilesForeground;
                }
            }

        }

    }

}

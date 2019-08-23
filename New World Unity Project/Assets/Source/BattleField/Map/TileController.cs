using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.BattleField.Map {

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
            Vector3 sidesRealPosition = new Vector3(surfaceRealPosition.x, surfaceRealPosition.y, -CoordinatesTransformation.HidingDifference);
            transform.position = CoordinatesTransformation.RealToVisible(sidesRealPosition);
            surface.transform.position = CoordinatesTransformation.RealToVisible(surfaceRealPosition, out int spriteOrder);
            surface.GetComponent<SpriteRenderer>().sortingOrder = spriteOrder + (int) SpriteLayers.Sublayers.TilesForeground;
            float surfaceBrightness = ZeroHeightSurfaceBrightness + (1 - ZeroHeightSurfaceBrightness) * (surfaceRealPosition.z / MapController.Instance.HeightLimit);
            surface.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 0, surfaceBrightness);
            int index = 0;
            bool lastSides = false;
            do {
                if (sidesRealPosition.z >= surfaceRealPosition.z - CoordinatesTransformation.HidingDifference) {
                    sidesRealPosition.z = surfaceRealPosition.z - CoordinatesTransformation.HidingDifference;
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
                sides.transform.position = CoordinatesTransformation.RealToVisible(sidesRealPosition);
                sides.GetComponent<SpriteRenderer>().sortingOrder = spriteOrder +
                    (int) (lastSides ? SpriteLayers.Sublayers.TilesBackground : SpriteLayers.Sublayers.TilesForeground);
                ++index;
                sidesRealPosition.z += CoordinatesTransformation.HidingDifference;
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

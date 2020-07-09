using NewWorld.Battle.Cores.Map;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Map {
    
    public class ClusterController : MonoBehaviour {

        // Fabric.

        public static ClusterController BuildCluster() => BuildCluster(null, Vector2Int.zero);

        public static ClusterController BuildCluster(MapPresentation presentation, Vector2Int startingPosition) {
            GameObject cluster = Instantiate(Prefab);
            ClusterController controller = cluster.GetComponent<ClusterController>();
            controller.StartingPosition = startingPosition;
            controller.Presentation = presentation;
            return controller;
        }

        protected static GameObject Prefab => PrefabSourceController.Instance.ClusterPrefab;


        // Fields.

        // Description.
        private MapPresentation presentation;
        private Vector2Int startingPosition;

        // Components.
        private Terrain terrain;
        private TerrainCollider terrainCollider;


        // Properties.

        public Vector2Int Size => new Vector2Int(
            Mathf.FloorToInt(terrain.terrainData.size.x),
            Mathf.FloorToInt(terrain.terrainData.size.z));

        public MapPresentation Presentation {
            get => presentation;
            set {
                presentation = value;
                Rebuild();
            }
        }

        public Vector2Int StartingPosition {
            get => startingPosition;
            set {
                startingPosition = value;
                Rebuild();
            }
        }


        // Life cycle.

        private void Awake() {
            terrain = GetComponent<Terrain>();
            GameObjects.ValidateComponent(terrain);
            terrain.enabled = false;
            terrainCollider = GetComponent<TerrainCollider>();
            GameObjects.ValidateComponent(terrainCollider);
            terrainCollider.enabled = false;
        }


        // Building.

        private void Rebuild() {
            if (presentation == null) {
                terrain.enabled = false;
                terrainCollider.enabled = false;
                return;
            }
            terrain.enabled = true;
            terrainCollider.enabled = true;

            // Set transform.
            transform.localPosition = new Vector3(startingPosition.x - 0.5f, 0, startingPosition.y - 0.5f);
            transform.localRotation = Quaternion.identity;

        }


    }

}

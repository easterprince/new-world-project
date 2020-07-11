using NewWorld.Battle.Cores.Generation.Map;
using NewWorld.Battle.Cores.Map;
using NewWorld.Utilities;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace NewWorld.Battle.Controllers.Map {
    
    public class ClusterController : MonoBehaviour {

        // Fabric.

        public static ClusterController BuildCluster() => BuildCluster(null, Vector2Int.zero);

        public static ClusterController BuildCluster(MapPresentation presentation, Vector2Int startingPosition) {

            // Instantiate GameObjct.
            GameObject cluster = Instantiate(Prefab);
            ClusterController controller = cluster.GetComponent<ClusterController>();

            // Force cloning of TerrainData.
            var terrainData = controller.terrain.terrainData;
            terrainData = Instantiate(terrainData);
            controller.terrain.terrainData = terrainData;
            controller.terrainCollider.terrainData = terrainData;

            // Set game info.
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
                gameObject.name = $"Cluster {startingPosition}";
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
            Vector3 startingPoint = new Vector3(startingPosition.x - 0.5f, 0, startingPosition.y - 0.5f);
            transform.localPosition = startingPoint;
            transform.localRotation = Quaternion.identity;

            // Set heightmap and holemap.
            int heightmapResolution = terrain.terrainData.heightmapResolution;
            var heightmap = new float[heightmapResolution, heightmapResolution]; // [0; 1] as height / heightLimit
            int holemapResolution = terrain.terrainData.holesResolution;
            var holemap = new bool[holemapResolution, holemapResolution]; // false if hole, true if surface
            foreach (var holeIndex in Enumerables.InRange2(holemapResolution)) {
                holemap[holeIndex.x, holeIndex.y] = true;
            }
            foreach (var pointIndex in Enumerables.InRange2(heightmapResolution)) {
                var localPoint = (Vector2) pointIndex / heightmapResolution * Size;
                float height = presentation.GetHeight(startingPoint + new Vector3(localPoint.x, 0, localPoint.y));
                if (height != float.NegativeInfinity) {
                    heightmap[pointIndex.y, pointIndex.x] = height / presentation.HeightLimit; // swap x and y!
                } else {
                    foreach (var difference in Enumerables.InRange2(2)) {
                        var holeIndex = pointIndex - difference;
                        if (Enumerables.IsInRange2(holeIndex, holemapResolution)) {
                            holemap[holeIndex.y, holeIndex.x] = false; // swap x and y!
                        }
                    }
                }
            }
            terrain.terrainData.SetHoles(0, 0, holemap);
            terrain.terrainData.SetHeights(0, 0, heightmap);

        }


    }

}

using NewWorld.Battle.Cores.Map;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Map {
    
    public class ClusterController : BuildableController {

        // Fabric.

        public static ClusterController StartBuildingCluster() => StartBuildingCluster(null, Vector2Int.zero);

        public static ClusterController StartBuildingCluster(MapPresentation presentation, Vector2Int startingPosition) {

            // Instantiate GameObjct.
            GameObject cluster = Instantiate(Prefab);
            ClusterController controller = cluster.GetComponent<ClusterController>();

            // Force duplicating TerrainData.
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
        private MapPresentation presentation = null;
        private Vector2Int startingPosition = Vector2Int.zero;

        // Components.
        private Terrain terrain;
        private TerrainCollider terrainCollider;

        // Tasks.
        private Task<(float[,], bool[,])> heightmapAndHolemapGeneration = null;
        private CancellationTokenSource heightmapAndHolemapCancellation = null;


        // Properties.

        public Vector2Int Size => new Vector2Int(
            Mathf.FloorToInt(terrain.terrainData.size.x),
            Mathf.FloorToInt(terrain.terrainData.size.z));

        public MapPresentation Presentation {
            get => presentation;
            set {
                presentation = value;
                StartRebuilding();
            }
        }

        public Vector2Int StartingPosition {
            get => startingPosition;
            set {
                startingPosition = value;
                gameObject.name = $"Cluster {startingPosition}";
                StartRebuilding();
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

        private void LateUpdate() {

            // Check if terrain data generation is on.
            if (heightmapAndHolemapGeneration != null) {
                if (heightmapAndHolemapGeneration.IsCompleted) {

                    // Set heightmap and holemap.
                    (var heightmap, var holemap) = heightmapAndHolemapGeneration.Result;
                    terrain.terrainData.SetHoles(0, 0, holemap);
                    terrain.terrainData.SetHeights(0, 0, heightmap);

                    Built = true;
                }
            }

        }

        private protected override void OnDestroy() {
            CancelHeightmapAndHolemapGeneration();
            base.OnDestroy();
        }


        // Building.

        private void StartRebuilding() {

            // Clearing.
            Built = false;
            if (presentation == null) {
                
                // Disable components.
                terrain.enabled = false;
                terrainCollider.enabled = false;
                
                return;
            }

            // Enable components.
            terrain.enabled = true;
            terrainCollider.enabled = true;

            // Set transform.
            Vector3 startingPoint = new Vector3(startingPosition.x - 0.5f, 0, startingPosition.y - 0.5f);
            transform.localPosition = startingPoint;
            transform.localRotation = Quaternion.identity;

            // Start generating terrain data.
            int holemapResolution = terrain.terrainData.holesResolution;
            int heightmapResolution = terrain.terrainData.heightmapResolution;
            var clusterSize = Size;
            CancelHeightmapAndHolemapGeneration();
            heightmapAndHolemapCancellation = new CancellationTokenSource();
            var cancellationToken = heightmapAndHolemapCancellation.Token;
            heightmapAndHolemapGeneration = Task.Run(() =>
                GenerateHeightmapAndHolemap(heightmapResolution, holemapResolution, clusterSize, cancellationToken));

        }

        private (float[,], bool[,]) GenerateHeightmapAndHolemap(
            int heightmapResolution, int holemapResolution, Vector2Int clusterSize, CancellationToken cancellationToken) {

            cancellationToken.ThrowIfCancellationRequested();

            // Initialize maps.
            Vector3 startingPoint = new Vector3(startingPosition.x - 0.5f, 0, startingPosition.y - 0.5f);
            var heightmap = new float[heightmapResolution, heightmapResolution]; // [0; 1] as height / heightLimit
            var holemap = new bool[holemapResolution, holemapResolution]; // false if hole, true if surface
            foreach (var holeIndex in Enumerables.InRange2(holemapResolution)) {
                holemap[holeIndex.x, holeIndex.y] = true;
            }
            cancellationToken.ThrowIfCancellationRequested();

            // Set heights.
            foreach (var pointIndex in Enumerables.InRange2(heightmapResolution)) {
                var localPoint = (Vector2) pointIndex / (heightmapResolution - 1) * clusterSize;
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
                cancellationToken.ThrowIfCancellationRequested();
            }

            return (heightmap, holemap);
        }


        // Support method.

        private void CancelHeightmapAndHolemapGeneration() {
            if (heightmapAndHolemapGeneration == null) {
                return;
            }

            heightmapAndHolemapCancellation.Cancel();
            heightmapAndHolemapCancellation = null;
            heightmapAndHolemapGeneration = null;

        }


    }

}

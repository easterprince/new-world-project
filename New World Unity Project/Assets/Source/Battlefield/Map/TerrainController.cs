using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Map {

    public class TerrainController : MonoBehaviour {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private int clusterSize;

        [SerializeField]
        private int heightMapResolution;

        [SerializeField]
        private int alphaMapResolution;

        [SerializeField]
        private float tileMaximumRadius;

#pragma warning restore IDE0044, CS0414, CS0649

        private TerrainLayer terrainLayer;

        private Vector2Int clustersCount;
        private Terrain[,] clusters;


        private void OnValidate() {
            clusterSize = Mathf.NextPowerOfTwo(Mathf.Clamp(clusterSize, 1, 32));
            heightMapResolution = Mathf.NextPowerOfTwo(Mathf.Clamp(heightMapResolution - 1, 32, 2048)) + 1;
            alphaMapResolution = Mathf.NextPowerOfTwo(Mathf.Clamp(heightMapResolution, 32, 2048));
            tileMaximumRadius = Mathf.Clamp(tileMaximumRadius, 0, 1);
        }


        // Life cycle.

        void Awake() {
            terrainLayer = Resources.Load("Default Terrain Layer") as TerrainLayer;
        }


        // Initialization.

        public void Load(MapDescription description) {

            int heightMapResolutionPerUnit = (heightMapResolution - 1) / clusterSize;
            clustersCount = new Vector2Int(
                    description.Size.x / clusterSize + (description.Size.x % clusterSize == 0 ? 0 : 1),
                    description.Size.y / clusterSize + (description.Size.y % clusterSize == 0 ? 0 : 1));
            clusters = new Terrain[clustersCount.x, clustersCount.y];

            foreach (Vector2Int clusterIndex in Enumerables.GetAllVectorsInRectangle(clustersCount)) {

                float[,] heightMap = new float[heightMapResolution, heightMapResolution];
                Vector2Int startPoint = clusterIndex * (heightMapResolution - 1);
                foreach (Vector2Int relativePoint in Enumerables.GetAllVectorsInRectangle(new Vector2Int(heightMapResolution, heightMapResolution))) {
                    Vector2 pointPosition = (Vector2) (startPoint + relativePoint) / heightMapResolutionPerUnit;
                    Vector2Int mainTile = Vector2Int.FloorToInt(pointPosition + new Vector2(tileMaximumRadius, tileMaximumRadius));
                    float heightSum = 0;
                    float weightSum = 0;
                    foreach (Vector2Int tile in Enumerables.GetAllVectorsInRectangle(mainTile, mainTile + new Vector2Int(2, 2))) {
                        float height = Mathf.Max(-1, description.GetSurfaceNode(tile)?.Height ?? -1);
                        float distance = MaximumMetric.GetMaximumNorm((Vector2) tile - pointPosition);
                        float weight = Mathf.Max(0, (1 - 2 * tileMaximumRadius) - distance);
                        heightSum += weight * height;
                        weightSum += weight;
                    }
                    float pointHeight = heightSum / weightSum;
                    heightMap[relativePoint.y, relativePoint.x] = pointHeight / description.HeightLimit;
                }

                float[,,] alphaMap = new float[alphaMapResolution, alphaMapResolution, 1];
                for (int x = 0; x < alphaMapResolution; ++x) {
                    for (int y = 0; y < alphaMapResolution; ++y) {
                        alphaMap[x, y, 0] = 1;
                    }
                }

                TerrainData terrainData = new TerrainData();
                terrainData.heightmapResolution = heightMapResolution;
                terrainData.alphamapResolution = alphaMapResolution;
                terrainData.baseMapResolution = 1024;
                terrainData.SetDetailResolution(1024, 16);
                terrainData.size = new Vector3(clusterSize, description.HeightLimit, clusterSize);
                terrainData.SetHeights(0, 0, heightMap);
                terrainData.terrainLayers = new TerrainLayer[] { terrainLayer };
                terrainData.SetAlphamaps(0, 0, alphaMap);

                GameObject terrainObject = Terrain.CreateTerrainGameObject(terrainData);
                terrainObject.transform.parent = transform;
                terrainObject.transform.position = new Vector3(clusterIndex.x * clusterSize, 0, clusterIndex.y * clusterSize);
                terrainObject.name = "Cluster " + terrainObject.transform.position;
                Terrain terrain = terrainObject.GetComponent<Terrain>();
                clusters[clusterIndex.x, clusterIndex.y] = terrain;

            }


        }


        // Information.

        public float GetSurfaceHeight(Vector2 position, float maximumRadius = 0) {
            return clusters[0, 0].SampleHeight(position);
        }


    }

}

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

        [SerializeField]
        private float flatBorder;

        [SerializeField]
        private float bottomLevel;

#pragma warning restore IDE0044, CS0414, CS0649

        private TerrainLayer terrainLayer;

        private Vector2Int clustersCount;
        private Terrain[,] clusters;


        private void OnValidate() {
            clusterSize = Mathf.NextPowerOfTwo(Mathf.Clamp(clusterSize, 1, 32));
            heightMapResolution = Mathf.NextPowerOfTwo(Mathf.Clamp(heightMapResolution - 1, 32, 2048)) + 1;
            alphaMapResolution = Mathf.NextPowerOfTwo(Mathf.Clamp(alphaMapResolution, 32, 2048));
            tileMaximumRadius = Mathf.Clamp(tileMaximumRadius, 0, 1);
            flatBorder = Mathf.Max(0.5f, flatBorder);
            bottomLevel = Mathf.Min(0, bottomLevel);
        }


        // Life cycle.

        void Awake() {
            terrainLayer = Resources.Load("Default Terrain Layer") as TerrainLayer;
        }


        // Initialization.

        public void Load(MapDescription description) {

            clustersCount = new Vector2Int(
                    Mathf.CeilToInt((description.Size.x - 1 + 2 * flatBorder) / clusterSize),
                    Mathf.CeilToInt((description.Size.y - 1 + 2 * flatBorder) / clusterSize)
            );
            clusters = new Terrain[clustersCount.x, clustersCount.y];

            int heightMapResolutionPerUnit = (heightMapResolution - 1) / clusterSize;
            int alphaMapResolutionPerUnit = alphaMapResolution / clusterSize;
            Vector2Int lastHeightPoint = clustersCount * (heightMapResolution - 1);
            Vector2Int lastAlphaPoint = clustersCount * alphaMapResolution - Vector2Int.one;

            foreach (Vector2Int clusterIndex in Enumerables.InRange2(clustersCount)) {

                float[,] heightMap = new float[heightMapResolution, heightMapResolution];
                Vector2Int startPoint = clusterIndex * (heightMapResolution - 1);
                foreach (Vector2Int relativePoint in Enumerables.InRange2(heightMapResolution)) {
                    Vector2Int point = startPoint + relativePoint;
                    float heightValue = 0;
                    if (point.x != 0 && point.y != 0 && point.x != lastHeightPoint.x && point.y != lastHeightPoint.y) {
                        Vector2 pointPosition = (Vector2) point / heightMapResolutionPerUnit - new Vector2(flatBorder, flatBorder);
                        Vector2Int mainTile = Vector2Int.FloorToInt(pointPosition + new Vector2(tileMaximumRadius, tileMaximumRadius));
                        float heightSum = 0;
                        float weightSum = 0;
                        foreach (Vector2Int tile in Enumerables.InRange2(mainTile, mainTile + new Vector2Int(2, 2))) {
                            float height = Mathf.Max(-1, description.GetSurfaceNode(tile)?.Height ?? -1);
                            float distance = MaximumMetric.GetNorm(tile - pointPosition);
                            float weight = Mathf.Max(0, (1 - 2 * tileMaximumRadius) - distance);
                            heightSum += weight * height;
                            weightSum += weight;
                        }
                        float pointHeight = heightSum / weightSum - bottomLevel;
                        heightValue = pointHeight / (description.HeightLimit - bottomLevel);
                    }
                    heightMap[relativePoint.y, relativePoint.x] = heightValue;
                }

                float[,,] alphaMap = new float[alphaMapResolution, alphaMapResolution, 1];
                startPoint = clusterIndex * alphaMapResolution;
                foreach (Vector2Int relativePoint in Enumerables.InRange2(alphaMapResolution)) {
                    Vector2Int point = startPoint + relativePoint;
                    float alphaValue = 0;
                    if (point.x != 0 && point.y != 0 && point.x != lastAlphaPoint.x && point.y != lastAlphaPoint.y) {
                        Vector2 pointPosition = (point + new Vector2(0.5f, 0.5f)) / alphaMapResolutionPerUnit - new Vector2(flatBorder, flatBorder);
                        float borderDistance = Mathf.Max(0, Mathf.Max(
                                Mathf.Abs(Mathf.Clamp(pointPosition.x, 0, description.Size.x - 1) - pointPosition.x),
                                Mathf.Abs(Mathf.Clamp(pointPosition.y, 0, description.Size.y - 1) - pointPosition.y)
                        ) - tileMaximumRadius);
                        alphaValue = Mathf.Max(0, 1 - borderDistance / (0.5f * flatBorder));
                    }
                    alphaMap[relativePoint.y, relativePoint.x, 0] = alphaValue;
                }

                TerrainData terrainData = new TerrainData();
                terrainData.heightmapResolution = heightMapResolution;
                terrainData.alphamapResolution = alphaMapResolution;
                terrainData.baseMapResolution = 1024;
                terrainData.SetDetailResolution(1024, 16);
                terrainData.size = new Vector3(clusterSize, description.HeightLimit - bottomLevel, clusterSize);
                terrainData.SetHeights(0, 0, heightMap);
                terrainData.terrainLayers = new TerrainLayer[] { terrainLayer };
                terrainData.SetAlphamaps(0, 0, alphaMap);

                GameObject terrainObject = Terrain.CreateTerrainGameObject(terrainData);
                terrainObject.transform.parent = transform;
                terrainObject.transform.position = new Vector3(
                        clusterIndex.x * clusterSize- flatBorder,
                        bottomLevel,
                        clusterIndex.y * clusterSize - flatBorder
                );
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

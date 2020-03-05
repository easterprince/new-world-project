using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NewWorld.Utilities;
using UnityEngine.Events;

namespace NewWorld.Battlefield.Map {

    public class TerrainController : MonoBehaviour {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [Header("Prefab")]
        [SerializeField]
        private GameObject clusterPrefab;

        [Header("Drawing parameters")]
        [SerializeField]
        private float tileMaximumRadius;
        [SerializeField]
        private float flatBorder;
        [SerializeField]
        private float abyssLevel;

#pragma warning restore IDE0044, CS0414, CS0649

        private bool constructed = true;
        private Terrain[,] clusters = new Terrain[0, 0];

        private int clusterSize;
        private int heightMapResolution;
        private int alphaMapResolution;


        private void OnValidate() {
            tileMaximumRadius = Mathf.Clamp(tileMaximumRadius, 0, 1);
            flatBorder = Mathf.Max(0.5f, flatBorder);
            abyssLevel = Mathf.Min(0, abyssLevel);
        }


        // Properties.

        public Vector2Int ClustersCount => new Vector2Int(clusters.GetLength(0), clusters.GetLength(1));


        // Life cycle.

        private void Start() {
            if (clusterPrefab == null) {
                throw new MissingReferenceException("Missing cluster prefab!");
            }
            Terrain terrain = clusterPrefab.GetComponent<Terrain>();
            if (terrain == null) {
                throw new MissingComponentException("Cluster prefab is missing Terrain component!");
            }
            TerrainData terrainData = terrain.terrainData;
            clusterSize = (int) terrainData.size.x;
            heightMapResolution = terrainData.heightmapResolution;
            alphaMapResolution = terrainData.alphamapResolution;
        }


        // Loading.

        public void StartReconstruction(MapDescription description, UnityAction afterAction = null) {
            // TODO. Make it possible to abort current reconstruction and begin new one.
            if (constructed == false) {
                throw new System.NotImplementedException();
            }
            constructed = false;
            StartCoroutine(Reconstruct(description, afterAction));
        }

        private IEnumerator Reconstruct(MapDescription description, UnityAction afterAction = null) {

            foreach (Terrain cluster in clusters) {
                Destroy(cluster.gameObject);
            }

            if (description != null) {

                Vector2Int clustersCount = new Vector2Int(
                        Mathf.CeilToInt((description.Size.x - 1 + 2 * flatBorder) / clusterSize),
                        Mathf.CeilToInt((description.Size.y - 1 + 2 * flatBorder) / clusterSize)
                );
                clusters = new Terrain[clustersCount.x, clustersCount.y];

                // TODO. For each cluster make its own task to achieve maximal cpu usage.
                Task<float[,][,]> heightMapCalculating = Task.Run(() => CalculateHeightMaps(description));
                Task<float[,][,,]> alphaMapCalculating = Task.Run(() => CalculateAlphaMaps(description));
                while (!heightMapCalculating.IsCompleted || !alphaMapCalculating.IsCompleted) {
                    yield return null;
                }
                float[,][,] heightMaps = heightMapCalculating.Result;
                float[,][,,] alphaMaps = alphaMapCalculating.Result;

                foreach (Vector2Int clusterIndex in Enumerables.InRange2(clustersCount)) {
                    float[,] heightMap = heightMaps[clusterIndex.x, clusterIndex.y];
                    float[,,] alphaMap = alphaMaps[clusterIndex.x, clusterIndex.y];

                    GameObject terrainObject = Instantiate(clusterPrefab, transform);
                    terrainObject.transform.position = new Vector3(
                            clusterIndex.x * clusterSize - flatBorder,
                            abyssLevel,
                            clusterIndex.y * clusterSize - flatBorder
                    );
                    terrainObject.layer = gameObject.layer;
                    terrainObject.name = "Cluster " + terrainObject.transform.position;
                    Terrain terrain = terrainObject.GetComponent<Terrain>();
                    clusters[clusterIndex.x, clusterIndex.y] = terrain;

                    terrain.terrainData = Instantiate(terrain.terrainData);
                    var terrainData = terrain.terrainData;
                    terrainData.size = new Vector3(clusterSize, description.HeightLimit - abyssLevel, clusterSize);
                    terrainData.SetHeights(0, 0, heightMap);
                    terrainData.SetAlphamaps(0, 0, alphaMap);

                }

                foreach (Vector2Int clusterIndex in Enumerables.InRange2(clustersCount)) {
                    clusters[clusterIndex.x, clusterIndex.y].SetNeighbors(
                        (clusterIndex.x == 0 ? null : clusters[clusterIndex.x - 1, clusterIndex.y]),
                        (clusterIndex.y == 0 ? null : clusters[clusterIndex.x, clusterIndex.y - 1]),
                        (clusterIndex.x == clustersCount.x - 1 ? null : clusters[clusterIndex.x + 1, clusterIndex.y]),
                        (clusterIndex.y == clustersCount.y - 1 ? null : clusters[clusterIndex.x, clusterIndex.y + 1])
                    );
                }

            } else {

                clusters = new Terrain[0, 0];

            }

            constructed = true;
            afterAction?.Invoke();
        }

        private float[,][,] CalculateHeightMaps(MapDescription description) {
            float[,][,] heightMaps = new float[ClustersCount.x, ClustersCount.y][,];

            int heightMapResolutionPerUnit = (heightMapResolution - 1) / clusterSize;
            Vector2Int lastHeightPoint = ClustersCount * (heightMapResolution - 1);

            foreach (Vector2Int clusterIndex in Enumerables.InRange2(ClustersCount)) {

                float[,] heightMap = new float[heightMapResolution, heightMapResolution];
                Vector2Int startPoint = clusterIndex * (heightMapResolution - 1);
                foreach (Vector2Int relativePoint in Enumerables.InRange2(heightMapResolution)) {
                    Vector2Int point = startPoint + relativePoint;
                    float heightValue = 0;
                    if (point.x != 0 && point.y != 0 && point.x != lastHeightPoint.x && point.y != lastHeightPoint.y) {
                        Vector2 pointPosition = (Vector2) point / heightMapResolutionPerUnit - new Vector2(flatBorder, flatBorder);
                        heightValue = (CalculateHeight(pointPosition, description) - abyssLevel) / (description.HeightLimit - abyssLevel);
                    }
                    heightMap[relativePoint.y, relativePoint.x] = heightValue;
                }
                heightMaps[clusterIndex.x, clusterIndex.y] = heightMap;

            }

            return heightMaps;
        }

        // TODO. Make abyss white.
        private float[,][,,] CalculateAlphaMaps(MapDescription description) {
            float[,][,,] alphaMaps = new float[ClustersCount.x, ClustersCount.y][,,];

            int alphaMapResolutionPerUnit = alphaMapResolution / clusterSize;
            Vector2Int lastAlphaPoint = ClustersCount * alphaMapResolution - Vector2Int.one;

            foreach (Vector2Int clusterIndex in Enumerables.InRange2(ClustersCount)) {

                float[,,] alphaMap = new float[alphaMapResolution, alphaMapResolution, 1];
                Vector2Int startPoint = clusterIndex * alphaMapResolution;
                foreach (Vector2Int relativePoint in Enumerables.InRange2(alphaMapResolution)) {
                    Vector2Int point = startPoint + relativePoint;
                    float alphaValue = 0;
                    if (point.x != 0 && point.y != 0 && point.x != lastAlphaPoint.x && point.y != lastAlphaPoint.y) {
                        Vector2 pointPosition = (point + new Vector2(0.5f, 0.5f)) / alphaMapResolutionPerUnit - new Vector2(flatBorder, flatBorder);
                        float borderDistance = Mathf.Max(0, Mathf.Max(
                                Mathf.Abs(Mathf.Clamp(pointPosition.x, 0, description.Size.x - 1) - pointPosition.x),
                                Mathf.Abs(Mathf.Clamp(pointPosition.y, 0, description.Size.y - 1) - pointPosition.y)
                        ) - tileMaximumRadius);
                        float borderAlphaValue = Mathf.Max(0, 1 - borderDistance / (0.5f * flatBorder));
                        float pointHeight = CalculateHeight(pointPosition, description);
                        float abyssAlphaValue = (Mathf.Min(pointHeight, 0) - abyssLevel) / -abyssLevel;
                        alphaValue = Mathf.Min(abyssAlphaValue, borderAlphaValue);
                    }
                    alphaMap[relativePoint.y, relativePoint.x, 0] = alphaValue;
                }
                alphaMaps[clusterIndex.x, clusterIndex.y] = alphaMap;

            }

            return alphaMaps;
        }


        // Height calculation.

        private float CalculateHeight(Vector2 position, MapDescription description) {
            Vector2Int mainTile = Vector2Int.FloorToInt(position);
            Vector2 betweenTilesPosition = position - mainTile;
            float[,] weights = new float[2, 2];
            foreach (Vector2Int tileDifference in Enumerables.InSegment2(1)) {
                weights[tileDifference.x, tileDifference.y] = 1;
            }
            for (int coordinate = 0; coordinate <= 1; ++coordinate) {
                float xDistance = Mathf.Max(Mathf.Abs(betweenTilesPosition.x - coordinate) - tileMaximumRadius, 0);
                weights[coordinate, 0] *= Mathf.Max(0, 1 - 2 * tileMaximumRadius - xDistance);
                weights[coordinate, 1] *= Mathf.Max(0, 1 - 2 * tileMaximumRadius - xDistance);
                float yDistance = Mathf.Max(Mathf.Abs(betweenTilesPosition.y - coordinate) - tileMaximumRadius, 0);
                weights[0, coordinate] *= Mathf.Max(0, 1 - 2 * tileMaximumRadius - yDistance);
                weights[1, coordinate] *= Mathf.Max(0, 1 - 2 * tileMaximumRadius - yDistance);
            }
            float heightSum = 0;
            float weightSum = 0;
            foreach (Vector2Int tileDifference in Enumerables.InSegment2(1)) {
                NodeDescription node = description[mainTile + tileDifference];
                float nodeHeight = (node.Type != NodeDescription.NodeType.Abyss ? node.Height : abyssLevel);
                heightSum += nodeHeight * weights[tileDifference.x, tileDifference.y];
                weightSum += weights[tileDifference.x, tileDifference.y];
            }
            float height = heightSum / weightSum;
            return height;
        }


        // Information.

        public float GetSurfaceHeight(Vector2 position) {
            return GetSurfaceHeight(new Vector3(position.x, 0, position.y));
        }

        public float GetSurfaceHeight(Vector3 position) {
            if (clusters.Length == 0 || constructed == false) {
                return abyssLevel;
            }
            Vector2Int clusterPosition = GetNearestClusterPosition(position);
            Terrain cluster = clusters[clusterPosition.x, clusterPosition.y];
            return cluster.SampleHeight(position) + cluster.transform.position.y;
        }


        // Support.

        private Vector2Int GetNearestClusterPosition(Vector3 position) {
            Vector3Int clusterPosition3 = Vector3Int.FloorToInt((position - clusters[0, 0].transform.position) / clusterSize);
            Vector2Int clusterPosition = new Vector2Int(clusterPosition3.x, clusterPosition3.z);
            clusterPosition.x = Mathf.Clamp(clusterPosition.x, 0, ClustersCount.x - 1);
            clusterPosition.y = Mathf.Clamp(clusterPosition.y, 0, ClustersCount.y - 1);
            return clusterPosition;
        }


    }

}

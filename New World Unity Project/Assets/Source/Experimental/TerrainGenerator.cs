using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Experimental {

    public class TerrainGenerator : MonoBehaviour {

        private int size;
        private int heightLimit;
        private int heightMapResolutionPerUnit;
        private int heightMapResolution;
        private int alphaMapResolution;

        private TerrainLayer terrainLayer;


        void Awake() {

            size = 16;
            heightLimit = 10;
            heightMapResolutionPerUnit = 32;
            heightMapResolution = size * heightMapResolutionPerUnit + 1;
            alphaMapResolution = size * heightMapResolutionPerUnit;

            terrainLayer = Resources.Load("Default Terrain Layer") as TerrainLayer;

            float[,] heightMap = new float[heightMapResolution, heightMapResolution];
            for (Vector2Int tilePosition =  Vector2Int.zero; tilePosition.x < size; ++tilePosition.x) {
                for (tilePosition.y = 0; tilePosition.y < size; ++tilePosition.y) {
                    float height = Random.Range(0f, 1f);
                    float normalizedHeight = height / heightLimit;
                    foreach (Vector2Int pointPosition in GetAllPointsOfTile(tilePosition)) {
                        heightMap[pointPosition.x, pointPosition.y] = normalizedHeight;
                    }
                }
            }
            foreach (Vector2Int point in GetAllGridPoints()) {
                float heightsSum = 0;
                int heightsCount = 0;
                foreach (Vector2Int neighbouringPoint in FilterTilePoints(GetAllNeighbouringPoints(point))) {
                    heightsSum += heightMap[neighbouringPoint.x, neighbouringPoint.y];
                    ++heightsCount;
                }
                heightMap[point.x, point.y] = heightsSum / heightsCount;
            }

            float[,,] alphaMap = new float[alphaMapResolution, alphaMapResolution, 1];
            for (int x = 0; x < alphaMapResolution; ++x) {
                for (int y = 0; y < alphaMapResolution; ++y) {
                    alphaMap[x, y, 0] = (x + y) % 2;
                }
            }

            TerrainData terrainData = new TerrainData();
            terrainData.heightmapResolution = heightMapResolution;
            terrainData.alphamapResolution = alphaMapResolution;
            terrainData.baseMapResolution = 1024;
            terrainData.SetDetailResolution(1024, 16);
            terrainData.size = new Vector3(size, heightLimit, size);
            terrainData.SetHeights(0, 0, heightMap);
            terrainData.terrainLayers = new TerrainLayer[] { terrainLayer };
            terrainData.SetAlphamaps(0, 0, alphaMap);

            GameObject terrainObject = Terrain.CreateTerrainGameObject(terrainData);
            terrainObject.name = "Terrain";
            terrainObject.transform.parent = transform;
            Terrain terrain = terrainObject.GetComponent<Terrain>();

        }

        private IEnumerable<Vector2Int> GetAllPoints() {
            Vector2Int point = Vector2Int.zero;
            for (point.x = 0; point.x < heightMapResolution; ++point.x) {
                for (point.y = 0; point.y < heightMapResolution; ++point.y) {
                    yield return point;
                }
            }
        }

        private IEnumerable<Vector2Int> GetAllGridPoints() {
            Vector2Int point = Vector2Int.zero;
            for (point.x = 0; point.x < heightMapResolution; point.x += heightMapResolutionPerUnit) {
                for (point.y = 0; point.y < heightMapResolution; ++point.y) {
                    yield return point;
                }
            }
            for (point.y = 0; point.y < heightMapResolution; point.y += heightMapResolutionPerUnit) {
                for (point.x = 0; point.x < heightMapResolution; ++point.x) {
                    if (point.x % heightMapResolutionPerUnit == 0) {
                        continue;
                    }
                    yield return point;
                }
            }
        }

        private IEnumerable<Vector2Int> GetAllPointsOfTile(Vector2Int tilePosition) {
            for (int dx = 1; dx < heightMapResolutionPerUnit; ++dx) {
                for (int dy = 1; dy < heightMapResolutionPerUnit; ++dy) {
                    yield return new Vector2Int(heightMapResolutionPerUnit * tilePosition.x + dx, heightMapResolutionPerUnit * tilePosition.y + dy);
                }
            }
        }

        private IEnumerable<Vector2Int> GetAllNeighbouringPoints(Vector2Int pointPosition) {
            Vector2Int neighbouringPoint = Vector2Int.zero;
            for (neighbouringPoint.x = pointPosition.x - 1; neighbouringPoint.x <= pointPosition.x + 1; ++neighbouringPoint.x) {
                for (neighbouringPoint.y = pointPosition.y - 1; neighbouringPoint.y <= pointPosition.y + 1; ++neighbouringPoint.y) {
                    if (neighbouringPoint.x < 0 || neighbouringPoint.x >= heightMapResolution || neighbouringPoint.y < 0 || neighbouringPoint.y >= heightMapResolution) {
                        continue;
                    }
                    yield return neighbouringPoint;
                }
            }
        }

        private IEnumerable<Vector2Int> FilterTilePoints(IEnumerable<Vector2Int> points) {
            foreach (Vector2Int point in points) {
                if (point.x % heightMapResolutionPerUnit == 0 || point.y % heightMapResolutionPerUnit == 0) {
                    continue;
                }
                yield return point;
            }
        }


    }

}

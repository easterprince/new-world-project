using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Experimental {

    public class TerrainGenerator : MonoBehaviour {

        void Awake() {
            Terrain terrain = GetComponent<Terrain>();
            TerrainData terrainData = new TerrainData();
            terrainData.size = new Vector3(10, 10, 10);
            terrainData.heightmapResolution = 65;
            float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
            for (int i = 0; i < terrainData.heightmapResolution; ++i) {
                for (int j = 0; j < terrainData.heightmapResolution; ++j) {
                    heightMap[i, j] = 0.1f * Random.Range(0f, 1f);
                }
            }
            terrainData.SetHeights(0, 0, heightMap);
            terrain.terrainData = terrainData;
        }

    }

}

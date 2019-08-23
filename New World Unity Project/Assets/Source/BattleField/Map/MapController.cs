using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Utilities.Singletones;

namespace NewWorld.BattleField.Map {

    public class MapController : SceneSingleton<MapController> {

        // Fields.

        private MapDescription description;
        private Vector2Int tilesCount;
        private TileController[,] tiles;


        // Properties.

        public Vector2Int Size => description.Size;
        public float HeightLimit => description.HeightLimit;


        // Life cycle.

        override protected void Awake() {
            base.Awake();
            Instance = this;
            description = BattlefieldLoader.Instance.MapDescription;
            tilesCount = new Vector2Int(2 * description.Size.x + 1, 2 * description.Size.y + 1);
            tiles = new TileController[tilesCount.x, tilesCount.y];
            for (int i = 0; i < tilesCount.x; ++i) {
                for (int j = 0; j < tilesCount.y; ++j) {
                    PlaceTile(i, j);
                }
            }
        }


        // Support.

        private void PlaceTilesNearbyNode(Vector2Int nodePosition) {
            int i = 2 * nodePosition.x + 1;
            int j = 2 * nodePosition.y + 1;
            for (int di = -1; di <= 1; ++di) {
                for (int dj = -1; dj <= 1; ++dj) {
                    PlaceTile(i + di, j + dj);
                }
            }
        }

        private void PlaceTile(int i, int j) {
            float x = 0.5f * i - 1;
            float y = 0.5f * j - 1;
            Vector2Int mainNodePosition = new Vector2Int((i + 1) / 2 - 1, (j + 1) / 2 - 1);
            float tileHeight = float.PositiveInfinity;
            for (int dx = 0; dx <= 1; ++dx) {
                for (int dy = 0; dy <= 1; ++dy) {
                    if (dx == 1 && (i & 1) == 1 || dy == 1 && (j & 1) == 1) {
                        continue;
                    }
                    NodeDescription node = description.GetSurfaceNode(new Vector2Int(mainNodePosition.x + dx, mainNodePosition.y + dy));
                    if (node == null) {
                        continue;
                    }
                    tileHeight = Mathf.Min(tileHeight, node.Height);
                }
            }
            TileController tile = tiles[i, j];
            if (tileHeight == float.PositiveInfinity) {
                if (tile != null) {
                    Destroy(tile.gameObject);
                }
            } else {
                if (tile == null) {
                    tile = TileController.BuildTile($"Tile ({i}, {j})");
                    tile.transform.parent = this.transform;
                    tiles[i, j] = tile;
                }
                tile.Place(new Vector3(x, y, tileHeight));
            }
        }



    }

}

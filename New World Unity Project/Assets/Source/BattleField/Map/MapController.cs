using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Loading;

namespace NewWorld.Battlefield.Map {

    public class MapController : SceneSingleton<MapController> {

        // Fields.

        private MapDescription description;
        private Vector2Int tilesCount;
        private TileController[,] tiles;
        private int currentDirection;


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
            DoForAllTiles(UpdateTileAtPosition);
            currentDirection = 0;
        }


        // Outer control.

        public void Rotate(int newDirection) {
            currentDirection = newDirection;
            DoForAllTiles((Vector2Int tileArrayPosition) => {
                TileController tile = tiles[tileArrayPosition.x, tileArrayPosition.y];
                if (tile != null) {
                    tile.Rotate(currentDirection);
                }
            });
        }


        // Support.

        delegate void TileAction(Vector2Int tileArrayPosition);

        private void DoForAdjacentTiles(Vector2Int nodePosition, TileAction action) {
            Vector2Int tileArrayPosition = new Vector2Int(2 * nodePosition.x + 1, 2 * nodePosition.y + 1);
            for (int dx = -1; dx <= 1; ++dx) {
                for (int dy = -1; dy <= 1; ++dy) {
                    action.Invoke(new Vector2Int(tileArrayPosition.x + dx, tileArrayPosition.y + dy));
                }
            }
        }

        private void DoForAllTiles(TileAction action) {
            for (int x = 0; x < tilesCount.x; ++x) {
                for (int y = 0; y < tilesCount.y; ++y) {
                    action.Invoke(new Vector2Int(x, y));
                }
            }
        }

        private void UpdateTileAtPosition(Vector2Int tileArrayPosition) {

            // Calculating height of the tile.
            Vector2Int mainNodePosition = new Vector2Int((tileArrayPosition.x + 1) / 2 - 1, (tileArrayPosition.y + 1) / 2 - 1);
            float tileHeight = float.PositiveInfinity;
            for (int dx = 0; dx <= 1; ++dx) {
                for (int dy = 0; dy <= 1; ++dy) {
                    if (dx == 1 && (tileArrayPosition.x & 1) == 1 || dy == 1 && (tileArrayPosition.y & 1) == 1) {
                        continue;
                    }
                    NodeDescription node = description.GetSurfaceNode(new Vector2Int(mainNodePosition.x + dx, mainNodePosition.y + dy));
                    if (node == null) {
                        continue;
                    }
                    tileHeight = Mathf.Min(tileHeight, node.Height);
                }
            }

            // Updating the tile.
            TileController tile = tiles[tileArrayPosition.x, tileArrayPosition.y];
            if (tileHeight == float.PositiveInfinity) {
                if (tile != null) {
                    Destroy(tile.gameObject);
                }
            } else {
                if (tile == null) {
                    tile = TileController.BuildTile($"Tile ({tileArrayPosition.x}, {tileArrayPosition.y})");
                    tile.transform.parent = this.transform;
                    tiles[tileArrayPosition.x, tileArrayPosition.y] = tile;
                }
                Vector2 tileRealPosition = new Vector2(0.5f * tileArrayPosition.x - 1, 0.5f * tileArrayPosition.y - 1);
                tile.Place(new Vector3(tileRealPosition.x, tileRealPosition.y, tileHeight));
            }

        }


    }

}

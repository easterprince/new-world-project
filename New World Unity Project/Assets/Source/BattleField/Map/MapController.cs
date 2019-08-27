using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Loading;
using NewWorld.Battlefield.Composition;

namespace NewWorld.Battlefield.Map {

    public partial class MapController : SceneSingleton<MapController> {

        // Fields.

        private MapDescription description;
        private Vector2Int tilesCount;
        private float[,] tileHeights;
        private TileController[,] tiles;
        private int currentVisionDirection;


        // Properties.

        public Vector2Int Size => description.Size;
        public float HeightLimit => description.HeightLimit;


        // Life cycle.

        override protected void Awake() {
            base.Awake();
            Instance = this;
            description = BattlefieldLoader.Instance.MapDescription;
            tilesCount = new Vector2Int(2 * description.Size.x + 1, 2 * description.Size.y + 1);
            tileHeights = new float[tilesCount.x, tilesCount.y];
            tiles = new TileController[tilesCount.x, tilesCount.y];
            currentVisionDirection = 0;
            ReinitializeHidingsObservation();
            DoForAllTiles(UpdateTileHeight);
            DoForAllTiles(UpdateTileAtPosition);
        }


        // Outer control.

        public void Rotate(int newVisionDirection) {
            if (!VisionDirections.IsValidDirection(newVisionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException("newVisionDirection", newVisionDirection);
            }
            currentVisionDirection = newVisionDirection;
            ReinitializeHidingsObservation();
            DoForAllTiles((Vector2Int tileArrayPosition) => {
                TileController tile = tiles[tileArrayPosition.x, tileArrayPosition.y];
                if (tile != null) {
                    tile.Rotate(currentVisionDirection, CalculateHidingHeight(tileArrayPosition));
                }
            });
        }


        // Foreach wannabes.

        delegate void TileAction(Vector2Int tileArrayPosition);

        private void DoForAdjacentTiles(Vector2Int nodePosition, TileAction action) {
            if (!IsValidNodePosition(nodePosition)) {
                throw BuildInvalidNodePositionException("nodePosition", nodePosition);
            }
            if (action == null) {
                return;
            }
            Vector2Int tileArrayPosition = new Vector2Int(2 * nodePosition.x + 1, 2 * nodePosition.y + 1);
            for (int dx = -1; dx <= 1; ++dx) {
                for (int dy = -1; dy <= 1; ++dy) {
                    action.Invoke(new Vector2Int(tileArrayPosition.x + dx, tileArrayPosition.y + dy));
                }
            }
        }

        private void DoForAllTiles(TileAction action) {
            if (action == null) {
                return;
            }
            for (int x = 0; x < tilesCount.x; ++x) {
                for (int y = 0; y < tilesCount.y; ++y) {
                    action.Invoke(new Vector2Int(x, y));
                }
            }
        }


        // Tiles management.

        private void UpdateTileHeight(Vector2Int tileArrayPosition) {
            if (!IsValidTileArrayPosition(tileArrayPosition)) {
                throw BuildInvalidTileArrayPositionException("tileArrayPosition", tileArrayPosition);
            }

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
            if (tileHeight == float.PositiveInfinity) {
                tileHeight = float.NegativeInfinity;
            }
            tileHeights[tileArrayPosition.x, tileArrayPosition.y] = tileHeight;
        }

        private void UpdateTileAtPosition(Vector2Int tileArrayPosition) {
            if (!IsValidTileArrayPosition(tileArrayPosition)) {
                throw BuildInvalidTileArrayPositionException("tileArrayPosition", tileArrayPosition);
            }

            float tileHeight = tileHeights[tileArrayPosition.x, tileArrayPosition.y];
            TileController tile = tiles[tileArrayPosition.x, tileArrayPosition.y];
            if (tileHeight == float.NegativeInfinity) {
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
                tile.Place(new Vector3(tileRealPosition.x, tileRealPosition.y, tileHeight), CalculateHidingHeight(tileArrayPosition));
            }
        }


        // Support.

        private bool IsValidTileArrayPosition(Vector2Int parameterValue) {
            return parameterValue.x >= 0 && parameterValue.x < tilesCount.x && parameterValue.y >= 0 && parameterValue.y < tilesCount.y;
        }

        private System.ArgumentOutOfRangeException BuildInvalidTileArrayPositionException(string parameterName, Vector2Int parameterValue) {
            return new System.ArgumentOutOfRangeException(
                parameterName,
                parameterValue,
                $"Position must be inside tile array, which size is {tilesCount}."
            );
        }

        private bool IsValidNodePosition(Vector2Int parameterValue) {
            return parameterValue.x >= 0 && parameterValue.x < description.Size.x && parameterValue.y >= 0 && parameterValue.y < description.Size.y;
        }

        private System.ArgumentOutOfRangeException BuildInvalidNodePositionException(string parameterName, Vector2Int parameterValue) {
            return new System.ArgumentOutOfRangeException(
                parameterName,
                parameterValue,
                $"Position must be inside node array, which size is {description.Size}."
            );
        }


    }

}

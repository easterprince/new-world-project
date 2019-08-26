using UnityEngine;
using NewWorld.Battlefield.Composition;

namespace NewWorld.Battlefield.Map {

    public partial class MapController {

        // Fields.

        private float?[,] tileHidingHeights;


        // Initialization.

        private void ReinitializeHidingsObservation() {
            tileHidingHeights = new float?[tilesCount.x, tilesCount.y];
        }


        // Hiding height calculation.

        private float CalculateHidingHeight(Vector2Int tileArrayPosition) {
            if (!IsValidTileArrayPosition(tileArrayPosition)) {
                return float.NegativeInfinity;
            }
            if (tileHidingHeights[tileArrayPosition.x, tileArrayPosition.y] == null) {
                Vector2Int visionDirectionDelta = VisionDirections.GetDirectionDelta(currentVisionDirection);
                float hidingHeight = Mathf.Max(
                    CalculateHidingPotential(tileArrayPosition - visionDirectionDelta) - 0.5f * CoordinatesTransformations.TileHidingHeightDifference,
                    Mathf.Min(
                        CalculateHidingPotential(tileArrayPosition - new Vector2Int(visionDirectionDelta.x, 0)) - 0.5f * CoordinatesTransformations.TileHidingHeightDifference,
                        CalculateHidingPotential(tileArrayPosition - new Vector2Int(0, visionDirectionDelta.y)) - 0.5f * CoordinatesTransformations.TileHidingHeightDifference
                    )
                );
                tileHidingHeights[tileArrayPosition.x, tileArrayPosition.y] = hidingHeight;
            }
            return tileHidingHeights[tileArrayPosition.x, tileArrayPosition.y].Value;
        }

        private float CalculateHidingPotential(Vector2Int tileArrayPosition) {
            if (!IsValidTileArrayPosition(tileArrayPosition)) {
                return float.NegativeInfinity;
            }
            return Mathf.Max(CalculateHidingHeight(tileArrayPosition), tileHeights[tileArrayPosition.x, tileArrayPosition.y]);
        }

    }

}
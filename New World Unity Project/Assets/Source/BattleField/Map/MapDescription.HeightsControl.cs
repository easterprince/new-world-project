using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Map {

    public partial class MapDescription {

        // Constants.

        private static readonly float safeHeightDifference = 1.1f * Mathf.Sqrt(6);
        private static readonly Vector2Int[] mainDirections = new Vector2Int[] {
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };


        // Fields.

        private bool[,,] isVisible;
        private int[,] visibleDirectionsCount;


        // Initialization.

        /// <summary>
        /// Use in constructor to initialize fields for heights control.
        /// </summary>
        private void InitializeHeightsControl() {
            isVisible = new bool[size.x, size.y, mainDirections.Length];
            visibleDirectionsCount = new int[size.x, size.y];
        }


        // Heights control methods.

        private void CalculateHeightLimits(Vector2Int targetedPosition, out float lowerLimit, out float upperLimit) {
            if (!IsPositionValid(targetedPosition)) {
                throw BuildInvalidPositionException("targetedPosition", targetedPosition);
            }

            upperLimit = heightLimit;
            lowerLimit = upperLimit;

            for (int direction = 0; direction < mainDirections.Length; ++direction) {
                Vector2Int delta = mainDirections[direction];

                // Adjust height of targeted node to save its own visibility.
                float minHeight = 0;
                int distance = 0;
                for (Vector2Int position = targetedPosition - delta; IsPositionValid(position); position -= delta) {
                    NodeDescription node = surface[position.x, position.y];
                    ++distance;
                    if (node != null) {
                        minHeight = Mathf.Max(minHeight, node.Height - distance * safeHeightDifference);
                    }
                }
                lowerLimit = Mathf.Min(lowerLimit, minHeight);

                // Adjust height of targeted node to save visibility of other nodes.
                distance = 0;
                for (Vector2Int position = targetedPosition + delta; IsPositionValid(position); position += delta) {
                    NodeDescription node = surface[position.x, position.y];
                    ++distance;
                    if (node != null) {
                        if (visibleDirectionsCount[position.x, position.y] == 1 && isVisible[position.x, position.y, direction]) {
                            upperLimit = Mathf.Min(upperLimit, node.Height + distance * safeHeightDifference);
                        }
                    }
                }

            }

            if (lowerLimit > upperLimit + 0.1) { 
                throw new System.Exception(
                    $"Calculated lower limit ({lowerLimit}) is significantly more than upper limit ({upperLimit}) for " +
                    $"targeted position ({targetedPosition}). Map may be corrupted."
                );
            }
            if (lowerLimit > upperLimit) {
                float middle = (lowerLimit + upperLimit) / 2;
                lowerLimit = middle;
                upperLimit = middle;
            }
        }

        private void RecalculateVisibility(Vector2Int updatedPosition) {
            if (!IsPositionValid(updatedPosition)) {
                throw BuildInvalidPositionException("updatedPosition", updatedPosition);
            }

            int bestVisibleDirection = -1;
            float lowerLimit = float.PositiveInfinity;

            NodeDescription updatedNode = surface[updatedPosition.x, updatedPosition.y];
            visibleDirectionsCount[updatedPosition.x, updatedPosition.y] = 0;

            for (int direction = 0; direction < mainDirections.Length; ++direction) {
                Vector2Int delta = mainDirections[direction];

                isVisible[updatedPosition.x, updatedPosition.y, direction] = false;

                // Adjust height of targeted node to save its own visibility.
                float minHeight = 0;
                int distance = 0;
                for (Vector2Int position = updatedPosition - delta; IsPositionValid(position); position -= delta) {
                    NodeDescription node = surface[position.x, position.y];
                    ++distance;
                    if (node != null) {
                        minHeight = Mathf.Max(minHeight, node.Height - distance * safeHeightDifference);
                    }
                }
                if (updatedNode != null && updatedNode.Height >= minHeight) {
                    isVisible[updatedPosition.x, updatedPosition.y, direction] = true;
                    ++visibleDirectionsCount[updatedPosition.x, updatedPosition.y];
                }
                if (minHeight <= lowerLimit) {
                    lowerLimit = minHeight;
                    bestVisibleDirection = direction;
                }

                // Adjust height of targeted node to save visibility of other nodes.
                if (updatedNode != null) {
                    minHeight = Mathf.Max(minHeight, updatedNode.Height);
                }
                for (Vector2Int position = updatedPosition + delta; IsPositionValid(position); position += delta) {
                    NodeDescription node = surface[position.x, position.y];
                    minHeight -= safeHeightDifference;
                    if (node != null) {
                        int visibleDirectionsCountBefore = visibleDirectionsCount[position.x, position.y];
                        bool isVisibleBefore = isVisible[position.x, position.y, direction];
                        bool isVisibleNow = (node.Height >= minHeight || visibleDirectionsCountBefore == 1 && isVisibleBefore);
                        isVisible[position.x, position.y, direction] = isVisibleNow;
                        visibleDirectionsCount[position.x, position.y] = visibleDirectionsCountBefore - (isVisibleBefore ? 1 : 0) + (isVisibleNow ? 1 : 0);
                        minHeight = Mathf.Max(minHeight, node.Height);
                    }
                }

            }

            if (updatedNode != null && visibleDirectionsCount[updatedPosition.x, updatedPosition.y] == 0) {
                isVisible[updatedPosition.x, updatedPosition.y, bestVisibleDirection] = true;
                ++visibleDirectionsCount[updatedPosition.x, updatedPosition.y];
            }

        }

    }

}
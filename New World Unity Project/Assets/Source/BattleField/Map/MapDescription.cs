using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Composition;

namespace NewWorld.Battlefield.Map {

    public partial class MapDescription {

        // Constants.

        private static readonly float safeHeightDifference = 1.1f * (CoordinatesTransformations.TileHidingHeightDifference / 2);


        // Fields.

        private readonly Vector2Int size;
        private readonly float heightLimit;
        private readonly NodeDescription[,] surface;


        // Properties.

        public Vector2Int Size => size;
        public float HeightLimit => heightLimit;


        // Constructor.

        public MapDescription(Vector2Int size, float heightLimit) {
            if (size.x <= 0 || size.y <= 0) {
                throw new System.ArgumentOutOfRangeException("size", size, $"Components of size must be positive.");
            }
            if (heightLimit < 0) {
                throw new System.ArgumentOutOfRangeException("heightLimit", heightLimit, $"Height limit must be non-negative.");
            }
            this.size = size;
            this.heightLimit = heightLimit;
            surface = new NodeDescription[size.x, size.y];
            InitializeHeightsControl();
        }


        // Node processing.

        public NodeDescription GetSurfaceNode(Vector2Int position) {
            if (!IsPositionValid(position) || surface[position.x, position.y] == null) {
                return null;
            }
            return new NodeDescription(surface[position.x, position.y]);
        }

        public bool TrySetSurfaceNode(Vector2Int position, NodeDescription description) {
            if (!IsPositionValid(position)) {
                throw BuildInvalidPositionException("position", position);
            }
            if (description != null) {
                CalculateHeightLimits(position, out float lowerLimit, out float upperLimit);
                if (Mathf.Clamp(description.Height, lowerLimit, upperLimit) != description.Height) {
                    return false;
                }
            }
            surface[position.x, position.y] = new NodeDescription(description);
            RecalculateVisibility(position);
            return true;
        }

        public float SetSurfaceNode(Vector2Int position, NodeDescription description) {
            if (!IsPositionValid(position)) {
                throw BuildInvalidPositionException("position", position);
            }
            description = new NodeDescription(description);
            if (description != null) {
                CalculateHeightLimits(position, out float lowerLimit, out float upperLimit);
                description.Height = Mathf.Clamp(description.Height, lowerLimit, upperLimit);
            }
            surface[position.x, position.y] = new NodeDescription(description);
            RecalculateVisibility(position);
            return description.Height;
        }


        // Support.

        private bool IsPositionValid(Vector2Int position) {
            return position.x >= 0 && position.x < size.x && position.y >= 0 && position.y < size.y;
        }

        private System.ArgumentOutOfRangeException BuildInvalidPositionException(string parameterName, Vector2Int position) {
            return new System.ArgumentOutOfRangeException(parameterName, position, $"Position must be inside map (size: {size})");
        }


    }

}

using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Map {

    public partial class MapDescription {

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
        }


        // Node processing.

        public NodeDescription GetSurfaceNode(Vector2Int position) {
            if (!IsPositionValid(position) || surface[position.x, position.y] == null) {
                return null;
            }
            return new NodeDescription(surface[position.x, position.y]);
        }

        public NodeDescription GetClosestSurfaceNode(Vector2 worldPosition) {
            Vector2Int position = GetClosestPosition(worldPosition);
            if (surface[position.x, position.y] == null) {
                return null;
            }
            return new NodeDescription(surface[position.x, position.y]);
        }

        public float SetSurfaceNode(Vector2Int position, NodeDescription description) {
            if (!IsPositionValid(position)) {
                throw BuildInvalidPositionException(nameof(position), position);
            }
            description = new NodeDescription(description);
            if (description != null) {
                description.Height = Mathf.Clamp(description.Height, 0, heightLimit);
            }
            surface[position.x, position.y] = new NodeDescription(description);
            return description.Height;
        }


        // Support.

        private bool IsPositionValid(Vector2Int position) {
            return position.x >= 0 && position.x < size.x && position.y >= 0 && position.y < size.y;
        }

        private Vector2Int GetClosestPosition(Vector2 worldPosition) {
            worldPosition.x = Mathf.Clamp(worldPosition.x, 0, size.x - 1);
            worldPosition.y = Mathf.Clamp(worldPosition.y, 0, size.y - 1);
            return Vector2Int.RoundToInt(worldPosition);
        }

        private System.ArgumentOutOfRangeException BuildInvalidPositionException(string parameterName, Vector2Int position) {
            return new System.ArgumentOutOfRangeException(parameterName, position, $"Position must be inside map (size: {size})");
        }


    }

}

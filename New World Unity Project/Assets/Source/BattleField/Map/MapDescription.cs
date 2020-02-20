using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Map {

    public class MapDescription {

        // Fields.

        private readonly float heightLimit = 0;
        private readonly NodeDescription[,] surface = new NodeDescription[0, 0];


        // Properties.

        public Vector2Int Size => new Vector2Int(surface.GetLength(0), surface.GetLength(1));
        public float HeightLimit => heightLimit;

        public NodeDescription this[Vector2Int position] {
            get {
                if (surface.Length == 0) {
                    return new NodeDescription();
                }
                position = GetNearestRealNodePosition(position);
                return surface[position.x, position.y];
            }
            set {
                ValidateRealNodePosition(position, nameof(value));
                value.Height = Mathf.Clamp(value.Height, 0, heightLimit);
                surface[position.x, position.y] = value;
            }
        }


        // Constructor.

        public MapDescription(Vector2Int size, float heightLimit) {
            size = Vector2Int.Max(size, Vector2Int.zero);
            heightLimit = Mathf.Max(heightLimit, 0);
            this.heightLimit = heightLimit;
            surface = new NodeDescription[size.x, size.y];
        }


        // Support.

        private bool IsRealNodePosition(in Vector2Int position) {
            return position.x >= 0 && position.x < Size.x && position.y >= 0 && position.y < Size.y;
        }

        private void ValidateRealNodePosition(in Vector2Int position, string parameterName) {
            if (!IsRealNodePosition(position)) {
                throw new System.ArgumentOutOfRangeException(
                    parameterName,
                    $"Node position must be real, i.e. in [0; {Size.x})x[0; {Size.y})."
                );
            }
        }

        private Vector2Int GetNearestRealNodePosition(Vector2 position) {
            position.x = Mathf.Clamp(position.x, 0, Size.x - 1);
            position.y = Mathf.Clamp(position.y, 0, Size.y - 1);
            return Vector2Int.RoundToInt(position);
        }


    }

}

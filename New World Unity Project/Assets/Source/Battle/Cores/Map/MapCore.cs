using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Map {

    public class MapCore : ConnectableCoreBase<MapCore, MapPresentation, BattlefieldPresentation> {

        // Enumerator.
        
        public enum InterpolationMode {
            None = 0,
            Bilinear
        }

        
        // Fields.

        private float heightLimit = 0;
        private MapNode[,] realNodes = new MapNode[0, 0];
        private InterpolationMode interpolation = InterpolationMode.Bilinear;


        // Constructors.

        public MapCore(Vector2Int size = new Vector2Int(), float heightLimit = 0) {
            ValidateSize(size, nameof(size));
            ValidateHeightLimit(heightLimit, nameof(heightLimit));
            HeightLimit = heightLimit;
            size = Vector2Int.Max(size, Vector2Int.zero);
            realNodes = new MapNode[size.x, size.y];
        }

        public MapCore(MapCore other) {
            heightLimit = other.heightLimit;
            realNodes = new MapNode[other.realNodes.GetLength(0), other.realNodes.GetLength(1)];
            other.realNodes.CopyTo(realNodes, 0);
        }


        // Properties.

        public Vector2Int Size => new Vector2Int(realNodes.GetLength(0), realNodes.GetLength(1));
        
        public float HeightLimit {
            get => heightLimit;
            set {
                ValidateHeightLimit(heightLimit, nameof(value));
                heightLimit = value;
                foreach (var position in Enumerables.InRange2(Size)) {
                    var node = this[position];
                    node.Height = Mathf.Min(node.Height, heightLimit);
                    this[position] = node;
                }
            }
        }

        public MapNode this[Vector2Int position] {
            get {
                if (realNodes.Length == 0) {
                    return new MapNode();
                }
                position = GetNearestRealPosition(position);
                return realNodes[position.x, position.y];
            }
            set {
                ValidateRealPosition(position, nameof(value));
                value.Height = Mathf.Clamp(value.Height, 0, heightLimit);
                realNodes[position.x, position.y] = value;
            }
        }

        public InterpolationMode Interpolation {
            get => interpolation;
            set => interpolation = value;
        }


        // Presentation generation.

        private protected override MapPresentation BuildPresentation() {
            return new MapPresentation(this);
        }

        
        // Cloning.

        public override MapCore Clone() {
            return new MapCore(this);
        }


        // Informational methods.

        public bool IsRealPosition(in Vector2Int position) {
            return position.x >= 0 && position.x < Size.x && position.y >= 0 && position.y < Size.y;
        }

        public Vector2Int GetNearestPosition(Vector3 point) {
            return new Vector2Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.z));
        }

        public Vector2Int GetNearestRealPosition(Vector3 point) {
            return GetNearestRealPosition(GetNearestPosition(point));
        }

        public Vector2Int GetNearestRealPosition(Vector2Int position) {
            position.x = Mathf.Clamp(position.x, 0, Size.x - 1);
            position.y = Mathf.Clamp(position.y, 0, Size.y - 1);
            return Vector2Int.RoundToInt(position);
        }

        public float GetHeight(Vector3 point) {
            switch (interpolation) {
                case InterpolationMode.Bilinear:
                    return GetHeightByInterpolation(point);
                case InterpolationMode.None:
                default:
                    return GetNearestNodeHeight(point);
            }
        }


        // Editing methods.

        public void Resize(Vector2Int newSize) {
            ValidateSize(newSize, nameof(newSize));
            var newRealNodes = new MapNode[newSize.x, newSize.y];
            foreach (var position in Enumerables.InRange2(newSize)) {
                if (position.x < Size.x && position.y < Size.y) {
                    realNodes[position.x, position.y] = newRealNodes[position.x, position.y];
                }
            }
            realNodes = newRealNodes;
        }


        // Support methods.

        private void ValidateHeightLimit(float value, string variableName) {
            if (float.IsNaN(value)) {
                throw new ArgumentOutOfRangeException(variableName, "Height limit must not be NaN.");
            }
        }

        private void ValidateSize(in Vector2Int value, string variableName) {
            if (value.x < 0 || value.y < 0) {
                throw new ArgumentException("Size must be vector with non-negative components.", variableName);
            }
        }

        private void ValidateRealPosition(in Vector2Int position, string parameterName) {
            if (!IsRealPosition(position)) {
                throw new System.ArgumentOutOfRangeException(
                    parameterName,
                    $"Node position must be real, i.e. in [0; {Size.x})x[0; {Size.y})."
                );
            }
        }

        private float GetNearestNodeHeight(Vector3 point) {
            var node = this[GetNearestPosition(point)];
            if (node.Type == MapNode.NodeType.Abyss) {
                return float.NegativeInfinity;
            }
            return node.Height;
        }

        private float GetHeightByInterpolation(Vector3 point) {
            var mainPosition = new Vector2Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.z));

            // Collect surrounding node heights.
            float minNonAbyssHeight = float.NegativeInfinity;
            var heights = new float[2, 2];
            foreach (var localPosition in Enumerables.InSegment2(1)) {
                var position = mainPosition + localPosition;
                var node = this[position];
                float localHeight;
                if (node.Type == MapNode.NodeType.Abyss) {
                    localHeight = float.NegativeInfinity;
                } else {
                    localHeight = node.Height;
                    minNonAbyssHeight = Mathf.Max(minNonAbyssHeight, localHeight);
                }
                heights[localPosition.x, localPosition.y] = localHeight;
            }

            // Calculate surface height.
            if (minNonAbyssHeight == float.NegativeInfinity) {
                return float.NegativeInfinity;
            }
            float surfaceHeight = 0;
            float xCoefficient = point.x - mainPosition.x;
            float yCoefficient = point.z - mainPosition.y;
            foreach (var localPosition in Enumerables.InSegment2(1)) {
                float localHeight = Mathf.Max(minNonAbyssHeight, heights[localPosition.x, localPosition.y]);
                surfaceHeight += localHeight *
                    (localPosition.x == 1 ? xCoefficient : 1 - xCoefficient) *
                    (localPosition.y == 1 ? yCoefficient : 1 - yCoefficient);
            }
            return surfaceHeight;
        }


    }

}

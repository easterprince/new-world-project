using UnityEngine;
using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Utilities;

namespace NewWorld.Battle.Cores.Map {

    public class MapCore : ConnectableCoreBase<MapPresentation, BattlefieldPresentation> {

        // Fields.

        private float heightLimit = 0;
        private MapNode[,] realNodes = new MapNode[0, 0];


        // Properties.

        public Vector2Int Size => new Vector2Int(realNodes.GetLength(0), realNodes.GetLength(1));
        
        public float HeightLimit {
            get => heightLimit;
            set {
                heightLimit = Mathf.Max(0f, value);
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


        // Constructor.

        public MapCore(BattlefieldPresentation parent, Vector2Int size = new Vector2Int(), float heightLimit = 0) : base(parent) {
            HeightLimit = heightLimit;
            size = Vector2Int.Max(size, Vector2Int.zero);
            realNodes = new MapNode[size.x, size.y];
        }


        // Presentation generation.

        private protected override MapPresentation BuildPresentation() {
            return new MapPresentation(this);
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


        // Support methods.

        private void ValidateRealPosition(in Vector2Int position, string parameterName) {
            if (!IsRealPosition(position)) {
                throw new System.ArgumentOutOfRangeException(
                    parameterName,
                    $"Node position must be real, i.e. in [0; {Size.x})x[0; {Size.y})."
                );
            }
        }


    }

}

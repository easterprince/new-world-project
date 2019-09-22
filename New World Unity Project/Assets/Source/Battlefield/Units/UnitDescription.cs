using UnityEngine;
using NewWorld.Battlefield.Composition;

namespace NewWorld.Battlefield.Units {

    public class UnitDescription {

        // Fields.

        private readonly Vector2Int connectedNode;
        private readonly float size;


        // Constructors.

        public UnitDescription(Vector2Int connectedNode, float size) {
            if (!CoordinatesTransformations.IsValidSize(size)) {
                throw CoordinatesTransformations.BuildInvalidSizeException(nameof(size), size);
            }
            this.connectedNode = connectedNode;
            this.size = size;
        }

        public UnitDescription(UnitDescription other) {
            this.connectedNode = other.connectedNode;
            this.size = other.size;
        }


        // Properties.

        public Vector2Int ConnectedNode => connectedNode;
        public float Size => size;


    }

}

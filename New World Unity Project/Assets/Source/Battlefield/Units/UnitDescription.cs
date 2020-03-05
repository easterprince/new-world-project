using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public class UnitDescription {

        // Fields.

        private readonly Vector2Int connectedNode;
        private readonly float maximumRadius;


        // Constructors.

        public UnitDescription(Vector2Int connectedNode, float maximumRadius) {
            if (maximumRadius < 0) {
                throw new System.ArgumentOutOfRangeException(nameof(maximumRadius), "Radius should be non-negative.");
            }
            this.connectedNode = connectedNode;
            this.maximumRadius = maximumRadius;
        }

        public UnitDescription(UnitDescription other) {
            this.connectedNode = other.connectedNode;
            this.maximumRadius = other.maximumRadius;
        }


        // Properties.

        public Vector2Int ConnectedNode => connectedNode;
        public float MaximumRadius => maximumRadius;


    }

}

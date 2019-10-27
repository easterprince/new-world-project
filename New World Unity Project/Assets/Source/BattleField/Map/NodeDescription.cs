using UnityEngine;

namespace NewWorld.Battlefield.Map {

    public class NodeDescription {

        private readonly float height;


        // Properties.

        public float Height => height;


        // ToString().

        public override string ToString() {
            return $"(Height: {height})";
        }


        // Constructors.

        public NodeDescription(float height = 0) {
            this.height = height;
        }

        public NodeDescription(NodeDescription other) {
            if (other == null) {
                throw new System.ArgumentNullException(nameof(other));
            }
            height = other.height;
        }

    }

}

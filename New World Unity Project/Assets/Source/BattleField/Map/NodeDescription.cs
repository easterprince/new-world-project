using UnityEngine;

namespace NewWorld.BattleField.Map {

    public class NodeDescription {

        private float height;


        // Properties.

        public float Height {
            get => height;
            set => height = value;
        }


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
                throw new System.ArgumentNullException("other");
            }
            height = other.height;
        }

    }

}

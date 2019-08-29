using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public class UnitDescription {

        // Fields.

        private Vector2Int currentNode;


        // Constructors.

        public UnitDescription(Vector2Int currentNode) {
            this.currentNode = currentNode;
        }

        public UnitDescription(UnitDescription other) {
            this.currentNode = other.currentNode;
        }


        // Properties.

        public Vector2Int CurrentNode => currentNode;


    }

}

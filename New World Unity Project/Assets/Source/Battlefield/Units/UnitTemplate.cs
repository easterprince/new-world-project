using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public class UnitTemplate {

        // Fields.

        private Vector2Int connectedNode;


        // Properties.

        public Vector2Int ConnectedNode {
            get => connectedNode;
            set => connectedNode = value;
        }


        // Constructors.

        public UnitTemplate() {}


    }

}

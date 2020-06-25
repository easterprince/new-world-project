using UnityEngine;

namespace NewWorld.Battlefield.Unit {

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

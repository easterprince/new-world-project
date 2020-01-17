using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions {
    
    public class RelocationAction : UnitAction {

        // Fields.

        private Vector2Int newConnectedNode;


        // Properties.

        public Vector2Int NewConnectedNode => newConnectedNode;


        // Constructor.

        public RelocationAction(Vector2Int newConnectedNode) : base() {
            this.newConnectedNode = newConnectedNode;
        }

    }

}

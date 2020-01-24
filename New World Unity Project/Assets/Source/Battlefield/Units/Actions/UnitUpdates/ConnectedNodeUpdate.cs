using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class ConnectedNodeUpdate : UnitUpdate {

        // Fields.

        private readonly Vector2Int newConnectedNode;


        // Properties.

        public Vector2Int NewConnectedNode => newConnectedNode;


        // Constructor.

        public ConnectedNodeUpdate(UnitController updatedUnit, Vector2Int newConnectedNode) : base(updatedUnit) {
            this.newConnectedNode = newConnectedNode;
        }


    }

}

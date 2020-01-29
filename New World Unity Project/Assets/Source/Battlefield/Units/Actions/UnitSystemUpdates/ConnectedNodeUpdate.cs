using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitSystemUpdates {

    public class ConnectedNodeUpdate : UnitSystemUpdate {

        // Fields.

        private readonly UnitController updatedUnit;
        private readonly Vector2Int newConnectedNode;


        // Properties.

        public UnitController UpdatedUnit => updatedUnit;
        public Vector2Int NewConnectedNode => newConnectedNode;


        // Constructor.

        public ConnectedNodeUpdate(UnitController updatedUnit, Vector2Int newConnectedNode) : base() {
            this.updatedUnit = updatedUnit ?? throw new System.ArgumentNullException(nameof(updatedUnit));
            this.newConnectedNode = newConnectedNode;
        }


    }

}

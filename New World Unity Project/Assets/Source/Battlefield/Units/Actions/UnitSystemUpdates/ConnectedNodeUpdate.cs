using NewWorld.Battlefield.Units;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitSystemUpdates {

    public class ConnectedNodeUpdate : UnitSystemUpdate {

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

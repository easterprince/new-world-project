using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitSystemUpdates {

    public class UpdateConnectedNode : UnitSystemUpdate {

        // Fields.

        private readonly UnitController unit;
        private readonly Vector2Int newConnectedNode;


        // Properties.

        public UnitController Unit => unit;
        public Vector2Int NewConnectedNode => newConnectedNode;


        // Constructor.

        public UpdateConnectedNode(UnitController unit, Vector2Int newConnectedNode) : base() {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
            this.newConnectedNode = newConnectedNode;
        }


    }

}

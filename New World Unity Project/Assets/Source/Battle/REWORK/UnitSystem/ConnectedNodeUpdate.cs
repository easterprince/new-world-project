using NewWorld.Battlefield.Unit;
using UnityEngine;

namespace NewWorld.Battlefield.UnitSystem {

    public class ConnectedNodeUpdate : UnitSystemUpdate {

        // Fields.

        private readonly UnitController unit;
        private readonly Vector2Int newConnectedNode;


        // Properties.

        public UnitController Unit => unit;
        public Vector2Int NewConnectedNode => newConnectedNode;


        // Constructor.

        public ConnectedNodeUpdate(UnitController unit, Vector2Int newConnectedNode) : base() {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
            this.newConnectedNode = newConnectedNode;
        }


    }

}

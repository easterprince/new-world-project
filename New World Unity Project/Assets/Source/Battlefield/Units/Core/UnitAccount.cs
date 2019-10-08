using UnityEngine;

namespace NewWorld.Battlefield.Units.Core {

    public class UnitAccount {

        // Fields.

        private readonly UnitCore core;


        // Constructor.

        public UnitAccount(UnitCore core) {
            this.core = core ?? throw new System.ArgumentNullException(nameof(core));
        }


        // Properties.

        public Vector2Int ConnectedNode => core.ConnectedNode;
        public float Size => core.MaximumRadius;


    }

}

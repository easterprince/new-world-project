using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Bodies {
    
    public class MovementAction : UnitAction {

        // Fields.

        private readonly Vector3 velocity;
        private readonly bool adjustRotation;


        // Constructor.

        public MovementAction(Vector3 velocity, bool adjustRotation = false) {
            this.velocity = velocity;
            this.adjustRotation = adjustRotation;
        }


        // Properties.

        public Vector3 Velocity => velocity;
        public bool AdjustRotation => adjustRotation;


    }

}

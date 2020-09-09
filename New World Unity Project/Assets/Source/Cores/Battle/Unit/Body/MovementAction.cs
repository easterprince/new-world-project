using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Body {

    public class MovementAction : UnitAction {

        // Fields.

        private readonly Vector3 positionChange;
        private readonly bool adjustRotation;
        private readonly bool adjustVelocity;


        // Constructor.

        public MovementAction(Vector3 positionChange, bool adjustRotation = false, bool adjustVelocity = true) {
            this.positionChange = positionChange;
            this.adjustRotation = adjustRotation;
            this.adjustVelocity = adjustVelocity;
        }


        // Properties.

        public Vector3 PositionChange => positionChange;
        public bool AdjustRotation => adjustRotation;
        public bool AdjustVelocity => adjustVelocity;


    }

}

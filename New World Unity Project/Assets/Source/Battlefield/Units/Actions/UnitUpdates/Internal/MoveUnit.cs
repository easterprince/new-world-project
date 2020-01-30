using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal {

    public class MoveUnit : InternalUnitUpdate {

        // Fields.

        private Vector2? positionChange;
        private Quaternion? rotationFromForward;


        // Properties.

        public Vector2? PositionChange => positionChange;
        public Quaternion? RotationFromForward => rotationFromForward;


        // Constructors.

        public MoveUnit(UnitController owner, Vector2? positionChange, Quaternion? rotationFromForward) : base(owner) {
            this.positionChange = positionChange;
            this.rotationFromForward = rotationFromForward;
        }


    }


}

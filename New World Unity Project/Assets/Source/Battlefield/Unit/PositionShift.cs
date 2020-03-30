using UnityEngine;

namespace NewWorld.Battlefield.Unit {

    public class PositionShift : UnitUpdate {

        // Fields.

        private Vector2 addition;
        private Quaternion? rotationFromForward;


        // Properties.

        public Vector2 Addition => addition;
        public Quaternion? RotationFromForward => rotationFromForward;


        // Constructors.

        public PositionShift(UnitController owner, Vector2 addition, Quaternion? rotationFromForward) : base(owner) {
            this.addition = addition;
            this.rotationFromForward = rotationFromForward;
        }


    }


}

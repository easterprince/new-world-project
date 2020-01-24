using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class TransformUpdate : UnitUpdate {

        // Fields.

        private readonly Vector3? newPosition;
        private readonly Quaternion? newRotation;


        // Properties.

        public Vector3? NewPosition => newPosition;
        public Quaternion? NewRotation => newRotation;


        // Constructor.

        public TransformUpdate(UnitController updatedUnit, Vector3? newPosition, Quaternion? newRotation) : base(updatedUnit) {
            this.newPosition = newPosition;
            this.newRotation = newRotation;
        }


    }

}

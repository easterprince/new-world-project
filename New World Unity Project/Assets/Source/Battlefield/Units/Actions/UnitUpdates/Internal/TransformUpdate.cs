using NewWorld.Battlefield.Units;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal {

    public class TransformUpdate : InternalUpdate {

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

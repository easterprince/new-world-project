using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions {

    public class PositionUpdate : UnitUpdate {

        // Fields.

        private readonly Vector3 newPosition;


        // Properties.

        public Vector3 NewPosition => newPosition;


        // Constructor.

        public PositionUpdate(UnitController updatedUnit, Vector3 newPosition) : base(updatedUnit) {
            this.newPosition = newPosition;
        }


    }

}

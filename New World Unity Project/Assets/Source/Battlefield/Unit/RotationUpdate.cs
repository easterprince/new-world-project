using UnityEngine;

namespace NewWorld.Battlefield.Unit {

    public class RotationUpdate : UnitUpdate {

        // Fields.

        private Quaternion rotation;


        // Properties.

        public Quaternion Rotation => rotation;


        // Constructors.

        public RotationUpdate(UnitController owner, Quaternion rotation) : base(owner) {
            this.rotation = rotation;
        }


    }

}

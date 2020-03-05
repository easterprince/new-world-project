using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal {
    
    public class SetRotation : InternalUnitUpdate {

        // Fields.

        private Quaternion rotation;


        // Properties.

        public Quaternion Rotation => rotation;


        // Constructors.

        public SetRotation(UnitController owner, Quaternion rotation) : base(owner) {
            this.rotation = rotation;
        }

    
    }

}

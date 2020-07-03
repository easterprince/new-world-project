using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Body {
    
    public class RotationAction : UnitAction {

        // Fields.

        private readonly Quaternion rotation;


        // Constructor.

        public RotationAction(Quaternion rotation) {
            this.rotation = rotation;
        }


        // Properties.

        public Quaternion Rotation => rotation;


    }

}

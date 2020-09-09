using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Body {

    public class BodyPresentation : UnitModulePresentationBase<BodyModule> {

        // Constructor.

        public BodyPresentation(BodyModule presented) : base(presented) { }


        // Properties.

        public Vector3 Position => Presented.Position;
        public Vector3 Velocity => Presented.Velocity;
        public Quaternion Rotation => Presented.Rotation;


    }

}
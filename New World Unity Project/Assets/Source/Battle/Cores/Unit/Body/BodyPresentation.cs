using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Body {

    public class BodyPresentation : UnitPresentationBase<BodyModule> {

        // Constructor.

        public BodyPresentation(BodyModule presented) : base(presented) { }


        // Properties.

        public Vector3 Position => Presented.Position;
        public Vector3 Velocity => Presented.Velocity;
        public Quaternion Rotation => Presented.Rotation;


    }

}
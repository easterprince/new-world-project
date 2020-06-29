using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Bodies {

    public class BodyPresentation : UnitModulePresentationBase<Body> {
        
        // Constructor.
        
        public BodyPresentation(Body presented) : base(presented) {}


        // Properties.

        public Vector3 Position => Presented.Position;
        public Vector3 Velocity => Presented.Velocity;
        public Quaternion Rotation => Presented.Rotation;


    }

}
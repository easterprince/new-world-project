using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions.Motions {

    public class MotionConditionPresentation : ConditionPresentation<MotionCondition, MotionConditionPresentation> {
        
        // Constructor.
        
        public MotionConditionPresentation(MotionCondition presented) : base(presented) {}


        // Properties.

        override public string Description => $"Moving to {Presented.Destination}";


    }

}

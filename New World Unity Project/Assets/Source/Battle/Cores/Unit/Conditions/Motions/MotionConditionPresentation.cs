using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Motions {

    public class MotionConditionPresentation : ConditionPresentationBase<MotionCondition> {

        // Properties.

        public Vector3 Destination => Presented.Destination;
        public float MovementPerSecond => Presented.MovementPerSecond;


        // Constructor.

        public MotionConditionPresentation(MotionCondition presented) : base(presented) {}


    }

}

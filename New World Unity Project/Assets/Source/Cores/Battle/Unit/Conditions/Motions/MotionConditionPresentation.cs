using System;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Conditions.Motions {

    public class MotionConditionPresentation :
        ConditionPresentationBase<IMotionConditionPresentation>, IMotionConditionPresentation {

        // Properties.

        public Vector3 Destination => Presented.Destination;
        public float MovementPerSecond => Presented.MovementPerSecond;


        // Constructor.

        public MotionConditionPresentation(IMotionConditionPresentation presented) : base(presented) {}


    }

}

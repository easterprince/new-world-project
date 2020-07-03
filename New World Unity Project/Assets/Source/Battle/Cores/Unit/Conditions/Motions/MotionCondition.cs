using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Motions {

    public abstract class MotionCondition : ConditionModuleBase<MotionCondition, MotionConditionPresentation> {

        // Properties.

        public abstract Vector3 Destination { get; }
        public abstract float MovementPerSecond { get; }


        // Presentation generation.

        private protected override MotionConditionPresentation BuildPresentation() {
            return new MotionConditionPresentation(this);
        }


    }

}

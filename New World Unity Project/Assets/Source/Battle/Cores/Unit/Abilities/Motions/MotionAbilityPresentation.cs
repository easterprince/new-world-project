using System;

namespace NewWorld.Battle.Cores.Unit.Abilities.Motions {

    public class MotionAbilityPresentation : AbilityPresentationBase<MotionAbility> {

        // Constructor.

        public MotionAbilityPresentation(MotionAbility presented) : base(presented) {}


        // Properties.

        public float MovementPerSecond => Presented.MovementPerSecond;


    }

}

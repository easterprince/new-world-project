using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Abilities.Motions {

    public class MotionAbilityPresentation : AbilityPresentationBase<IMotionAbility>, IMotionAbilityPresentation {

        // Constructor.

        public MotionAbilityPresentation(IMotionAbility presented) : base(presented) {}


        // Properties.

        public float MovementPerSecond => Presented.MovementPerSecond;


        // Usage.

        public bool CheckIfUsable(Vector3 destination) => Presented.CheckIfUsable(destination);


    }

}

using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Abilities.Motions {

    public abstract class MotionAbility : AbilityModuleBase<MotionAbility, MotionAbilityPresentation> {

        // Properties.

        public abstract float MovementPerSecond { get; }


        // Presentation generation.

        private protected override MotionAbilityPresentation BuildPresentation() {
            return new MotionAbilityPresentation(this);
        }


        // Usage.

        public abstract bool CheckIfUsable(Vector3 destination);
        public abstract void Use(Vector3 destination);


    }

}

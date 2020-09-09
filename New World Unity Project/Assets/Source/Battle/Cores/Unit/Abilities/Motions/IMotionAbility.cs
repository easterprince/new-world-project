using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Abilities.Motions {

    public interface IMotionAbility : IAbilityModule, IMotionAbilityPresentation {

        // Presentation.

        new IMotionAbilityPresentation Presentation { get; }


        // Cloning.

        new IMotionAbility Clone();


        // Usage.

        void Use(Vector3 destination);


    }

}

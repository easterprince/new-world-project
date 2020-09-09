using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Abilities.Motions {

    public interface IMotionAbility : IAbilityModule, IMotionAbilityPresentation {

        // Presentation.

        new IMotionAbilityPresentation Presentation { get; }


        // Cloning.

        new IMotionAbility Clone();


        // Usage.

        void Use(Vector3 destination);


    }

}

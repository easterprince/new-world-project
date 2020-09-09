using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Abilities.Motions {
    
    public interface IMotionAbilityPresentation : IAbilityPresentation {

        // Properties.

        float MovementPerSecond { get; }


        // Usage.

        bool CheckIfUsable(Vector3 destination);


    }

}

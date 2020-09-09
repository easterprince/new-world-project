using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Abilities.Motions {
    
    public interface IMotionAbilityPresentation : IAbilityPresentation {

        // Properties.

        float MovementPerSecond { get; }


        // Usage.

        bool CheckIfUsable(Vector3 destination);


    }

}

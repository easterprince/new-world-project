using System;

namespace NewWorld.Battle.Cores.Unit.Abilities {
    
    public interface IAbilityPresentation : IOwnerPointer {

        // Properties.

        string Name { get; }
        string Description { get; }


    }

}

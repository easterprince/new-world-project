using System;

namespace NewWorld.Battle.Cores.Unit.Abilities {
    
    public interface IAbilityPresentation : IOwnerPointer {

        string Name { get; }
        string Description { get; }


    }

}

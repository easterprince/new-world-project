using System;

namespace NewWorld.Battle.Cores.Unit.Abilities {

    public abstract class AbilityModuleBase<TSelf, TAbilityPresentation> :
        UnitModuleBase<TSelf, TAbilityPresentation, UnitPresentation>, IAbility
        where TSelf : AbilityModuleBase<TSelf, TAbilityPresentation>
        where TAbilityPresentation : IOwnerPointer, IAbility {

        // Properties.

        public abstract string Description { get; }


    }

}

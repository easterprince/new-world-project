using NewWorld.Battle.Cores.Unit.AbilityCollection;
using System;

namespace NewWorld.Battle.Cores.Unit.Abilities {

    public abstract class AbilityModuleBase<TSelf, TAbilityPresentation> :
        UnitModuleBase<TSelf, TAbilityPresentation, AbilityCollectionPresentation>, IAbilityModule
        where TSelf : AbilityModuleBase<TSelf, TAbilityPresentation>
        where TAbilityPresentation : class, IAbilityPresentation {

        // Properties.

        public abstract string Name { get; }
        public abstract string Description { get; }

        IAbilityPresentation ICore<IAbilityModule, IAbilityPresentation>.Presentation => Presentation;


        // Cloning.

        IAbilityModule ICore<IAbilityModule, IAbilityPresentation>.Clone() => Clone();


    }

}

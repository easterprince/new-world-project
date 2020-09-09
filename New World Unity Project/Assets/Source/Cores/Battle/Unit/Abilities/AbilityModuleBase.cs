using NewWorld.Cores.Battle.Unit.AbilityCollection;

namespace NewWorld.Cores.Battle.Unit.Abilities {

    public abstract class AbilityModuleBase<TSelf, TAbilityPresentation> :
        UnitModuleBase<TSelf, TAbilityPresentation, AbilityCollectionPresentation>, IAbilityModule
        where TSelf : IAbilityModule
        where TAbilityPresentation : class, IAbilityPresentation {

        // Properties.

        public abstract string Name { get; }
        public abstract string Description { get; }

        IAbilityPresentation ICore<IAbilityModule, IAbilityPresentation>.Presentation => Presentation;


        // Cloning.

        IAbilityModule ICore<IAbilityModule, IAbilityPresentation>.Clone() => Clone();


    }

}

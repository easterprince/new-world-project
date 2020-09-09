using NewWorld.Cores.Battle.Unit.AbilityCollection;

namespace NewWorld.Cores.Battle.Unit.Abilities {
    
    public interface IAbilityModule :
        IUnitModule<IAbilityModule, IAbilityPresentation, AbilityCollectionPresentation>, IAbilityPresentation {}

}

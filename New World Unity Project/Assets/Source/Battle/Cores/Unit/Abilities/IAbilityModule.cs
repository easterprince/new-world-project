using NewWorld.Battle.Cores.Unit.AbilityCollection;

namespace NewWorld.Battle.Cores.Unit.Abilities {
    
    public interface IAbilityModule :
        IUnitModule<IAbilityModule, IAbilityPresentation, AbilityCollectionPresentation>, IAbilityPresentation {}

}

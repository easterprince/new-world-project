using NewWorld.Battle.Cores.Unit.Durability;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {
    
    public interface IAttackAbility : IAbilityModule, IAttackAbilityPresentation {

        // Presentation.

        new IAttackAbilityPresentation Presentation { get; }


        // Cloning.

        new IAttackAbility Clone();


        // Usage.

        void Use(UnitPresentation target);


    }

}

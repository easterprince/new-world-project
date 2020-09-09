namespace NewWorld.Cores.Battle.Unit.Abilities.Attacks {

    public interface IAttackAbility : IAbilityModule, IAttackAbilityPresentation {

        // Presentation.

        new IAttackAbilityPresentation Presentation { get; }


        // Cloning.

        new IAttackAbility Clone();


        // Usage.

        void Use(UnitPresentation target);


    }

}

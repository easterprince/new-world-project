using NewWorld.Cores.Battle.Unit.Durability;

namespace NewWorld.Cores.Battle.Unit.Abilities.Attacks {

    public interface IAttackAbilityPresentation : IAbilityPresentation {

        // Properties.

        Damage DamagePerSecond { get; }
        float AttackRange { get; }


        // Usage.

        bool CheckIfUsable(UnitPresentation target);


    }

}

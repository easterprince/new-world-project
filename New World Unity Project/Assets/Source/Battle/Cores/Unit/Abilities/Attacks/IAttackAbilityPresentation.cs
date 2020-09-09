using NewWorld.Battle.Cores.Unit.Durability;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {
    
    public interface IAttackAbilityPresentation : IAbilityPresentation {

        // Properties.

        Damage DamagePerSecond { get; }
        float AttackRange { get; }


        // Usage.

        bool CheckIfUsable(UnitPresentation target);


    }

}

using NewWorld.Cores.Battle.Unit.Durability;

namespace NewWorld.Cores.Battle.Unit.Conditions.Attacks {

    public interface IAttackConditionPresentation : IConditionPresentation {

        // Properties.

        UnitPresentation Target { get; }
        Damage DamagePerSecond { get; }
        float AttackRange { get; }


    }

}

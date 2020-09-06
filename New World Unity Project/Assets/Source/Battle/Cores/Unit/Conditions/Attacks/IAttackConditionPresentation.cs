using NewWorld.Battle.Cores.Unit.Durability;

namespace NewWorld.Battle.Cores.Unit.Conditions.Attacks {

    public interface IAttackConditionPresentation : IConditionPresentation {

        // Properties.

        UnitPresentation Target { get; }
        Damage DamagePerSecond { get; }


    }

}

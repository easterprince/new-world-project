using NewWorld.Battle.Cores.Unit.Durability;
using System;

namespace NewWorld.Battle.Cores.Unit.Conditions.Attacks {

    public abstract class AttackCondition : ConditionModuleBase<AttackCondition, AttackConditionPresentation> {

        // Properties.

        public abstract UnitPresentation Target { get; }
        public abstract Damage DamagePerSecond { get; }


        // Presentation generation.

        private protected override AttackConditionPresentation BuildPresentation() {
            return new AttackConditionPresentation(this);
        }


    }

}

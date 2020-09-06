using NewWorld.Battle.Cores.Unit.Durability;
using System;

namespace NewWorld.Battle.Cores.Unit.Conditions.Attacks {

    public class AttackConditionPresentation : ConditionPresentationBase<IAttackConditionPresentation> {

        // Properties.

        public UnitPresentation Target => Presented.Target;
        public Damage DamagePerSecond => Presented.DamagePerSecond;


        // Constructor.

        public AttackConditionPresentation(IAttackConditionPresentation presented) : base(presented) {}


    }

}

using NewWorld.Battle.Cores.Unit.Durability;
using System;

namespace NewWorld.Battle.Cores.Unit.Conditions.Attacks {

    public class AttackConditionPresentation :
        ConditionPresentationBase<IAttackConditionPresentation>, IAttackConditionPresentation {

        // Properties.

        public UnitPresentation Target => Presented.Target;
        public Damage DamagePerSecond => Presented.DamagePerSecond;
        public float AttackRange => Presented.AttackRange;


        // Constructor.

        public AttackConditionPresentation(IAttackConditionPresentation presented) : base(presented) {}


    }

}

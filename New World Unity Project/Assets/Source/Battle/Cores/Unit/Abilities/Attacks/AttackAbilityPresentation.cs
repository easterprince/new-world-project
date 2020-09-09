using NewWorld.Battle.Cores.Unit.Durability;
using System;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {

    public class AttackAbilityPresentation :
        AbilityPresentationBase<IAttackAbilityPresentation>, IAttackAbilityPresentation {
        
        // Constructor.
        
        public AttackAbilityPresentation(IAttackAbilityPresentation presented) : base(presented) {}


        // Properties.

        public Damage DamagePerSecond => Presented.DamagePerSecond;
        public float AttackRange => Presented.AttackRange;

        
        // Methods.
        
        public bool CheckIfUsable(UnitPresentation target) => Presented.CheckIfUsable(target);


    }

}

using NewWorld.Cores.Battle.Unit.Durability;
using System;

namespace NewWorld.Cores.Battle.Unit.Abilities.Attacks {

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

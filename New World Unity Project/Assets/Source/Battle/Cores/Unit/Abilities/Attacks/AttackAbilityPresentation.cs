using NewWorld.Battle.Cores.Unit.Durability;
using System;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {

    public class AttackAbilityPresentation : AbilityPresentationBase<AttackAbility> {
        
        // Constructor.
        
        public AttackAbilityPresentation(AttackAbility presented) : base(presented) {}


        // Properties.

        public Damage DamagePerSecond => Presented.DamagePerSecond;


    }

}

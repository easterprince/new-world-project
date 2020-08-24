using NewWorld.Battle.Cores.Unit.Durability;
using System;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {
    
    public abstract class AttackAbility : AbilityModuleBase<AttackAbility, AttackAbilityPresentation> {

        // Properties.

        public abstract Damage DamagePerSecond { get; }
        public abstract float EffectiveRange { get; }


        // Presentation generation.

        private protected override AttackAbilityPresentation BuildPresentation() {
            return new AttackAbilityPresentation(this);
        }


        // Usage.

        public abstract bool CheckIfUsable(UnitPresentation target);
        public abstract void Use(UnitPresentation target);



    }

}

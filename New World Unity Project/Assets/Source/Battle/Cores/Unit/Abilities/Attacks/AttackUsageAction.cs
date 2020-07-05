using System;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {
    
    public class AttackUsageAction : AbilityUsageActionBase<AttackAbilityPresentation> {

        // Fields.

        private readonly UnitPresentation target;


        // Constructor.

        public AttackUsageAction(AttackAbilityPresentation ability, UnitPresentation target) : base(ability) {
            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }


        // Properties.

        public UnitPresentation Target => target;

    
    }

}

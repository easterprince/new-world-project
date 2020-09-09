using System;

namespace NewWorld.Battle.Cores.Unit.Abilities {
    
    public abstract class AbilityUsageActionBase<TAbilityPresentation> : UnitAction
        where TAbilityPresentation : class, IAbilityPresentation {

        // Fields.

        private readonly TAbilityPresentation ability;


        // Constructor.

        public AbilityUsageActionBase(TAbilityPresentation ability) {
            this.ability = ability ?? throw new ArgumentNullException(nameof(ability));
        }


        // Properties.

        public TAbilityPresentation Ability => ability;


    }

}

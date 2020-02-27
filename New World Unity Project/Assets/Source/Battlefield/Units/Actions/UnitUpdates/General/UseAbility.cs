using UnityEngine;
using NewWorld.Battlefield.Units.Abilities;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class UseAbility : GeneralUnitUpdate {

        // Fields.
        
        private readonly IAbilityPresentation abilityPresentation;
        private readonly object parameterSet;


        // Properties.

        public IAbilityPresentation AbilityPresentation => abilityPresentation;
        public object ParameterSet => parameterSet;


        // Constructor.

        public UseAbility(IAbilityPresentation abilityPresentation, object parameterSet) : base(abilityPresentation.Owner) {
            this.abilityPresentation = abilityPresentation ?? throw new System.ArgumentNullException(nameof(abilityPresentation));
            this.parameterSet = parameterSet;
        }


    }

}

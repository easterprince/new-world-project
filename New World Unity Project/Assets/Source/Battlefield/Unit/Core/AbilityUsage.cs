using UnityEngine;
using NewWorld.Battlefield.Unit.Abilities;

namespace NewWorld.Battlefield.Unit.Core {

    public class AbilityUsage : GeneralUnitUpdate {

        // Fields.

        private readonly UnitAbility ability;
        private readonly object parameterSet;


        // Properties.

        public UnitAbility Ability => ability;
        public object ParameterSet => parameterSet;


        // Constructor.

        public AbilityUsage(UnitAbility ability, object parameterSet) : base(ability.Owner) {
            this.ability = ability ?? throw new System.ArgumentNullException(nameof(ability));
            this.parameterSet = parameterSet;
        }


    }

}

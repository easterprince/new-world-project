using UnityEngine;
using NewWorld.Battlefield.Units.Abilities.Active;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class AbilityUsage : UnitUpdate {

        // Fields.

        private readonly ActiveAbility ability;
        private readonly object parameterSet;


        // Properties.

        public ActiveAbility Ability => ability;
        public object ParameterSet => parameterSet;


        // Constructor.

        public AbilityUsage(ActiveAbility ability, object parameterSet) : base(ability.Owner) {
            this.ability = ability;
            this.parameterSet = parameterSet;
        }


    }

}

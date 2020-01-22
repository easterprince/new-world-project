using UnityEngine;
using NewWorld.Battlefield.Units.Abilities.Active;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public abstract class AbilityUsage : UnitUpdate {

        // Fields.

        private readonly ActiveAbility ability;
        private readonly object parameterSet;


        // Properties.

        public ActiveAbility Ability => ability;
        public object ParameterSet => parameterSet;


        // Constructor.

        public AbilityUsage(UnitController updatedUnit, ActiveAbility ability, object parameterSet) : base(updatedUnit) {
            if (updatedUnit != ability.Owner) {
                throw new System.ArgumentException("Updated unit must be owner of ability.");
            }
            this.ability = ability;
            this.parameterSet = parameterSet;
        }


    }

}

using UnityEngine;
using NewWorld.Battlefield.Units.Abilities.Active;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class AbilityUsage : UnitUpdate {

        // Fields.

        private readonly UsableAbility ability;


        // Properties.

        public UsableAbility Ability => ability;


        // Constructor.

        public AbilityUsage(UnitController updatedUnit, UsableAbility ability) : base(updatedUnit) {
            if (updatedUnit != ability.Owner) {
                throw new System.ArgumentException("Updated unit must be owner of ability.");
            }
            this.ability = ability;
        }


    }

    public class AbilityUsage<UsageParameterType> : UnitUpdate {

        // Fields.

        private readonly UsableAbility<UsageParameterType> ability;


        // Properties.

        public UsableAbility<UsageParameterType> Ability => ability;


        // Constructor.

        public AbilityUsage(UnitController updatedUnit, UsableAbility<UsageParameterType> ability) : base(updatedUnit) {
            if (updatedUnit != ability.Owner) {
                throw new System.ArgumentException("Updated unit must be owner of ability.");
            }
            this.ability = ability;
        }


    }

}

using UnityEngine;
using NewWorld.Battlefield.Units.Abilities.Active;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class AbilityUsage : AbilityActivation {

        // Properties.

        public UsableAbility UsableAbility => Ability as UsableAbility;


        // Constructor.

        public AbilityUsage(UnitController updatedUnit, UsableAbility ability) : base(updatedUnit, ability) {}


    }

    public class AbilityUsage<UsageParameterType> : AbilityActivation {

        // Fields.

        private readonly UsageParameterType usageParameter;


        // Properties.

        public UsableAbility<UsageParameterType> UsableAbility => Ability as UsableAbility<UsageParameterType>;
        public UsageParameterType UsageParameter => usageParameter;


        // Constructor.

        public AbilityUsage(UnitController updatedUnit, UsableAbility<UsageParameterType> ability, UsageParameterType usageParameter) : base(updatedUnit, ability) {
            this.usageParameter = usageParameter;
        }


    }

}

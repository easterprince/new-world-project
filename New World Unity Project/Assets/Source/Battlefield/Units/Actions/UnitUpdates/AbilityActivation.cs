using UnityEngine;
using NewWorld.Battlefield.Units.Abilities.Active;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public abstract class AbilityActivation : UnitUpdate {

        // Fields.

        private readonly ActiveAbility ability;


        // Properties.

        public ActiveAbility Ability => ability;


        // Constructor.

        public AbilityActivation(UnitController updatedUnit, ActiveAbility ability) : base(updatedUnit) {
            if (updatedUnit != ability.Owner) {
                throw new System.ArgumentException("Updated unit must be owner of ability.");
            }
            this.ability = ability;
        }


    }

}

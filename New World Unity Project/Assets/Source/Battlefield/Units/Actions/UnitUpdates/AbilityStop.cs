using UnityEngine;
using NewWorld.Battlefield.Units.Abilities.Active;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class AbilityStop : UnitUpdate {

        // Fields.

        private readonly ActiveAbility ability;
        private readonly bool forceStop;


        // Properties.

        public ActiveAbility Ability => ability;
        public bool ForceStop => forceStop;


        // Constructor.

        public AbilityStop(UnitController updatedUnit, ActiveAbility ability, bool forceStop) : base(updatedUnit) {
            if (updatedUnit != ability.Owner) {
                throw new System.ArgumentException("Updated unit must be owner of ability.");
            }
            this.ability = ability;
            this.forceStop = forceStop;
        }


    }


}

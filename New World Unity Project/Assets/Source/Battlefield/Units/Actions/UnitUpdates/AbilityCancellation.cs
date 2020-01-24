using UnityEngine;
using NewWorld.Battlefield.Units.Abilities.Active;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class AbilityCancellation : AbilityStop {

        // Constructor.

        public AbilityCancellation(ActiveAbility ability) : base(ability, false) {}


    }


}

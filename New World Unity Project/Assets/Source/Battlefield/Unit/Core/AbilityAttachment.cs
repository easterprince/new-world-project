using NewWorld.Battlefield.Unit.Abilities;
using NewWorld.Battlefield.Unit.Actions.UnitUpdates.General;
using UnityEngine;

namespace NewWorld.Battlefield.Unit.Core {

    public class AbilityAttachment : GeneralUnitUpdate {

        // Fields.

        private UnitAbility ability;


        // Properties.

        public UnitAbility Ability => ability;


        // Constructors.

        public AbilityAttachment(UnitController unit, UnitAbility ability) : base(unit) {
            this.ability = ability ?? throw new System.ArgumentNullException(nameof(ability));
        }


    }

}

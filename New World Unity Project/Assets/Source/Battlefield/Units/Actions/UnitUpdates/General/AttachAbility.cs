using NewWorld.Battlefield.Units.Abilities;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class AttachAbility : GeneralUnitUpdate {
        
        // Fields.

        private UnitAbility ability;


        // Properties.

        public UnitAbility Ability => ability;


        // Constructors.

        public AttachAbility(UnitController unit, UnitAbility ability) : base(unit) {
            this.ability = ability ?? throw new System.ArgumentNullException(nameof(ability));
        }


    }

}

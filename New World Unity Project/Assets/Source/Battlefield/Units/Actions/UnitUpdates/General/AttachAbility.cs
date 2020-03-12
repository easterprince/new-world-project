using NewWorld.Battlefield.Units.Abilities;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class AttachAbility : GeneralUnitUpdate {
        
        // Fields.

        private Ability ability;


        // Properties.

        public Ability Ability => ability;


        // Constructors.

        public AttachAbility(UnitController unit, Ability ability) : base(unit) {
            this.ability = ability ?? throw new System.ArgumentNullException(nameof(ability));
        }


    }

}

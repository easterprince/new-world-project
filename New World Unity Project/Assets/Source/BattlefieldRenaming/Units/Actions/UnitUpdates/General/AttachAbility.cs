using NewWorld.Battlefield.Units.Abilities;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class AttachAbility : GeneralUnitUpdate {
        
        // Fields.

        private IAbility ability;


        // Properties.

        public IAbility Ability => ability;


        // Constructors.

        public AttachAbility(UnitController unit, IAbility ability) : base(unit) {
            this.ability = ability ?? throw new System.ArgumentNullException(nameof(ability));
        }


    }

}

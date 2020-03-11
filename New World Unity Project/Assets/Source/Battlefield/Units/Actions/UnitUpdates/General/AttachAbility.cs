using NewWorld.Battlefield.Units.Abilities;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class AttachAbility : GeneralUnitUpdate {
        
        // Fields.

        private IUnitModule module;


        // Properties.

        public IUnitModule Module => module;


        // Constructors.

        public AttachAbility(UnitController unit, IAbility ability) : base(unit) {
            this.ability = ability ?? throw new System.ArgumentNullException(nameof(ability));
        }


    }

}

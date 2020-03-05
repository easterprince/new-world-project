using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class CauseDamage : GeneralUnitUpdate {

        // Fields.

        private readonly float damageValue;


        // Properties.

        public float DamageValue => damageValue;


        // Constructor.

        public CauseDamage(UnitController updatedUnit, float damageValue) : base(updatedUnit) {
            this.damageValue = damageValue;
        }


    }

}

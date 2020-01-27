using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class DamageCausing : UnitUpdate {

        // Fields.

        private readonly float damageValue;


        // Properties.

        public float DamageValue => damageValue;


        // Constructor.

        public DamageCausing(UnitController updatedUnit, float damageValue) : base(updatedUnit) {
            this.damageValue = damageValue;
        }


    }

}

using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class DamageCausing : UnitUpdate {

        // Fields.

        private float damageValue;


        // Properties.

        public float DamageValue => damageValue;


        // Constructor.

        public DamageCausing(UnitController owner, float damageValue) : base(owner) {
            this.damageValue = damageValue;
        }


    }

}

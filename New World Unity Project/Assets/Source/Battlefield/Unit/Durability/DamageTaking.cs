using NewWorld.Battlefield.Unit.Actions.UnitUpdates.General;
using UnityEngine;

namespace NewWorld.Battlefield.Unit.Durability {

    public class DamageTaking : GeneralUnitUpdate {

        // Fields.

        private readonly float damageValue;


        // Properties.

        public float DamageValue => damageValue;


        // Constructor.

        public DamageTaking(UnitController updatedUnit, float damageValue) : base(updatedUnit) {
            this.damageValue = damageValue;
        }


    }

}

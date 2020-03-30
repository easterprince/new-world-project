using NewWorld.Battlefield.Unit;
using UnityEngine;

namespace NewWorld.Battlefield.UnitSystem {

    public class UnitRemoval : UnitSystemUpdate {

        // Fields.

        private readonly UnitController unit;


        // Properties.

        public UnitController Unit => unit;


        // Constructor.

        public UnitRemoval(UnitController unit) : base() {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
        }


    }

}

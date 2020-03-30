using UnityEngine;

namespace NewWorld.Battlefield.Unit {

    public class UnitUpdate : GameAction {

        // Fields.

        private readonly UnitController unit;


        // Properties.

        public UnitController Unit => unit;


        // Constructor.

        public UnitUpdate(UnitController unit) {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
        }


    }

}

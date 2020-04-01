using UnityEngine;

namespace NewWorld.Battlefield.Unit {

    public class UnitAction<TSelf> : GameAction<TSelf> {

        // Properties.

        public UnitController Unit { get; }


        // Constructor.

        public UnitAction(UnitController unit) {
            Unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
        }


    }

}

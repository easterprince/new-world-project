using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitSystemUpdates {
    
    public class RemoveUnit : UnitSystemUpdate {

        // Fields.

        private readonly UnitController unit;


        // Properties.

        public UnitController Unit => unit;


        // Constructor.

        public RemoveUnit(UnitController unit) : base() {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
        }

    
    }

}

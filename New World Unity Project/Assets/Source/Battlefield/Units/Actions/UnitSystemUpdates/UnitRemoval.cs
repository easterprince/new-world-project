using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitSystemUpdates {
    
    public class UnitRemoval : UnitSystemUpdate {

        // Fields.

        private readonly UnitController removedUnit;


        // Properties.

        public UnitController RemovedUnit => removedUnit;


        // Constructor.

        public UnitRemoval(UnitController removedUnit) : base() {
            this.removedUnit = removedUnit ?? throw new System.ArgumentNullException(nameof(removedUnit));
        }

    
    }

}

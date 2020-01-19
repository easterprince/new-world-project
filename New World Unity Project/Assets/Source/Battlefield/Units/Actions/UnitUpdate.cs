using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions {

    public class UnitUpdate : GameAction {

        // Fields.

        private readonly UnitController updatedUnit;


        // Properties.

        public UnitController UpdatedUnit => updatedUnit;


        // Constructor.
    
        public UnitUpdate(UnitController updatedUnit) {
            if (updatedUnit == null) {
                throw new System.ArgumentNullException(nameof(updatedUnit));
            }
            this.updatedUnit = updatedUnit;
        }


    }

}

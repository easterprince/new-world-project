using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitSystemUpdates {

    public class UnitSystemUpdate : GameAction {

        // Fields.

        private readonly UnitController updatedUnit;


        // Properties.

        public UnitController UpdatedUnit => updatedUnit;


        // Constructor.

        public UnitSystemUpdate(UnitController updatedUnit) {
            if (updatedUnit == null) {
                throw new System.ArgumentNullException(nameof(updatedUnit));
            }
            this.updatedUnit = updatedUnit;
        }


    }

}

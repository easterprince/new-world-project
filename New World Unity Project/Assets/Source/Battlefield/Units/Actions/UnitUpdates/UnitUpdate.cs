using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class UnitUpdate : GameAction {

        // Fields.

        private readonly UnitController updatedUnit;


        // Properties.

        public UnitController UpdatedUnit => updatedUnit;


        // Constructor.

        public UnitUpdate(UnitController updatedUnit) {
            this.updatedUnit = updatedUnit ?? throw new System.ArgumentNullException(nameof(updatedUnit));
        }


    }

}

using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitSystemUpdates {

    public class AddUnit : UnitSystemUpdate {

        // Fields.

        private readonly UnitDescription description;


        // Properties.

        public UnitDescription Description => description;


        // Constructor.

        public AddUnit(UnitDescription description) : base() {
            this.description = description ?? throw new System.ArgumentNullException(nameof(description));
        }


    }

}

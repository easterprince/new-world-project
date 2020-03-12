using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitSystemUpdates {

    public class AddUnit : UnitSystemUpdate {

        // Fields.

        private readonly UnitTemplate description;


        // Properties.

        public UnitTemplate Description => description;


        // Constructor.

        public AddUnit(UnitTemplate description) : base() {
            this.description = description ?? throw new System.ArgumentNullException(nameof(description));
        }


    }

}

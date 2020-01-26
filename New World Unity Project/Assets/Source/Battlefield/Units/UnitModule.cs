using UnityEngine;

namespace NewWorld.Battlefield.Units {
    
    public class UnitModule {

        // Fields.

        private readonly UnitController owner;


        // Properties.

        public UnitController Owner => owner;


        // Constructor.

        public UnitModule(UnitController owner) {
            if (owner == null) {
                throw new System.ArgumentNullException(nameof(owner));
            }
            this.owner = owner;
        }


    }

}

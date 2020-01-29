using UnityEngine;

namespace NewWorld.Battlefield.Units {
    
    public class UnitModule {

        // Fields.

        private readonly UnitController owner;


        // Properties.

        public UnitController Owner => owner;


        // Constructor.

        public UnitModule(UnitController owner) {
            this.owner = owner ?? throw new System.ArgumentNullException(nameof(owner));
        }


    }

}

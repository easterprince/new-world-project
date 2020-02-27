using UnityEngine;

namespace NewWorld.Battlefield.Units {
    
    public abstract class UnitModule<PresentationType> : IUnitModule {

        // Fields.

        private UnitController owner = null;


        // Properties.

        public UnitController Owner => owner;
        public bool Connected => !(owner is null);
        public abstract PresentationType Presentation { get; }


        // Constructor.

        protected UnitModule() {}


        // Methods.

        protected void Connect(UnitController owner) {
            if (!(this.owner is null)) {
                throw new System.InvalidOperationException("Module has been already connected, can't do it again.");
            }
            if (owner is null) {
                throw new System.ArgumentNullException(nameof(owner));
            }
            this.owner = owner;
        }


    }

}

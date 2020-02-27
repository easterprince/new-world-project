using UnityEngine;

namespace NewWorld.Battlefield.Units {
    
    public abstract class UnitModule<PresentationType> : IUnitModule
        where PresentationType : class, IUnitModulePresentation {

        // Fields.

        private UnitController owner = null;
        private PresentationType presentation;


        // Properties.

        public UnitController Owner => owner;
        public bool Connected => !(owner is null);
        
        public PresentationType Presentation {
            get => presentation ?? throw new System.NullReferenceException("Presentation is not set.");
            protected set => presentation = value;
        }

        IUnitModulePresentation IUnitModule.Presentation => Presentation;


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

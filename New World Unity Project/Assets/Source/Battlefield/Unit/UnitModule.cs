using UnityEngine;

namespace NewWorld.Battlefield.Unit {

    public abstract class UnitModule {

        // Properties.

        public abstract bool Connected { get; }
        
        public abstract UnitController Owner { get; }


    }

    public abstract class UnitModule<TSelf, TParent> : UnitModule
        where TSelf : UnitModule<TSelf, TParent>
        where TParent : class {

        // Fields.

        private TParent parent;


        // Properties.
        
        override public bool Connected => !(parent is null);
        
        override public UnitController Owner {
            get {
                if (parent is UnitController owner) {
                    return owner;
                }
                if (parent is UnitModule module) {
                    return module.Owner;
                }
                return null;
            }
        }


        // Constructor.

        protected private UnitModule() {}


        // Public methods.

        public void Connect(TParent parent) {
            if (Connected) {
                throw new System.InvalidOperationException("Module has been already connected, can't do it again.");
            }
            this.parent = parent;
        }

        public void Disconnect() {
            if (!Connected) {
                throw new System.InvalidOperationException("Module has been already disconnected, can't do it again.");
            }
            this.parent = null;
        }

        // TODO: Clone().
        //public abstract TSelf Clone();


        // Connection validators.

        protected private void ValidateConnection() {
            if (Owner is null) {
                throw new System.InvalidOperationException("Module must be connected!");
            }
        }

        protected private void ValidateOwnership() {
            if (Owner is null) {
                throw new System.InvalidOperationException("Module must have owner!");
            }
        }


    }

    public abstract class UnitModule<TSelf, TParent, TPresentation> : UnitModule<TSelf, TParent>
        where TSelf : UnitModule<TSelf, TParent, TPresentation>
        where TParent : class
        where TPresentation : UnitModulePresentation<TSelf> {

        // Fields.

        private TPresentation presentation;


        // Properties.

        public TPresentation Presentation {
            get {
                if (presentation is null) {
                    presentation = BuildPresentation();
                    if (presentation is null) {
                        throw new System.NullReferenceException("Presentation building method returned null reference.");
                    }
                }
                return presentation;
            }
        }


        // Methods.

        protected private abstract TPresentation BuildPresentation();


    }


}

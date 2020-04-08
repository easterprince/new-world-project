using NewWorld.Battlefield.Unit.Controller;
using System.Collections.Generic;

namespace NewWorld.Battlefield.Unit {

    public abstract class UnitModuleBase<TSelf, TParent, TPresentation> : IChildModule<TParent>, IParentModule<TSelf>
        where TSelf : UnitModuleBase<TSelf, TParent, TPresentation>
        where TParent : class, IParentModule<TParent>
        where TPresentation : PresentationBase<TSelf> {

        // Fields.

        private TParent parent;
        private TPresentation presentation;
        private HashSet<IChildModule<TSelf>> children;


        // Properties.

        public UnitController Owner => parent.Owner;

        public bool Connected => !(parent is null);

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


        // Constructor.

        protected private UnitModuleBase() {}


        // Public methods.

        public void Connect(TParent parent) {
            if (parent is null) {
                throw new System.ArgumentNullException(nameof(parent));
            }
            if (Connected && this.parent == parent) {
                throw new System.InvalidOperationException("Module is already connected to another parent.");
            }
            this.parent = parent;
        }

        public void Disconnect() {
            if (!Connected) {
                return;
            }
            parent = null;
        }

        public void Attach(IChildModule<TSelf> module) {
            if (module is null) {
                throw new System.ArgumentNullException(nameof(module));
            }
            if (HasChild(module)) {
                return;
            }
            if (children == null) {
                children = new HashSet<IChildModule<TSelf>>();
            }
            children.Add(module);
        }

        public void Detach(IChildModule<TSelf> module) {
            if (module is null) {
                throw new System.ArgumentNullException(nameof(module));
            }
            if (!HasChild(module)) {
                return;
            }
            children.Remove(module);
        }

        public bool HasChild(IChildModule<TSelf> module) {
            if (children == null) {
                return false;
            }
            return children.Contains(module);
        }

        public abstract void ProcessAction(GameAction action);

        // TODO: Clone().
        //public abstract TSelf Clone();


        // Connection validator.

        protected private void ValidateOwnership() {
            if (Owner is null) {
                throw new System.InvalidOperationException($"Module must have owner!");
            }
        }


        // Presentation builder.

        protected private abstract TPresentation BuildPresentation();


    }

}

using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public abstract class UnitModule<TParent> : IUnitModule
        where TParent : class {

        // Fields.

        private ParentPassport<TParent> parentPassport = null;


        // Properties.

        public TParent Parent => parentPassport.Parent;
        
        public bool Connected => !(Parent is null);
        
        public UnitController Owner {
            get {
                if (Parent is UnitController owner) {
                    return owner;
                }
                if (Parent is IUnitModule module) {
                    return module.Owner;
                }
                return null;
            }
        }


        // Constructor.

        private protected UnitModule() {}


        // Methods.

        private protected void Connect(ParentPassport<TParent> parentPassport) {
            if (Connected) {
                throw new System.InvalidOperationException("Module has been already connected, can't do it again.");
            }
            if (parentPassport is null) {
                throw new System.ArgumentNullException(nameof(parentPassport));
            }
            this.parentPassport = parentPassport;
        }

        private protected void Disconnect(ParentPassport<TParent> parentPassport) {
            ValidatePassport(parentPassport);
            if (!Connected) {
                throw new System.InvalidOperationException("Module has been already disconnected, can't do it again.");
            }
            this.parentPassport = null;
        }

        private protected void ValidatePassport(ParentPassport<TParent> passport) {
            if (passport == null || passport != parentPassport) {
                throw new System.InvalidOperationException("Operation may be performed only by parent!");
            }
        }

        private protected void ValidatePassportOrDisconnection(ParentPassport<TParent> passport) {
            if (parentPassport != null && passport != parentPassport) {
                throw new System.InvalidOperationException("Operation may be performed only by parent or when disconnected!");
            }
        }


    }

    public abstract class UnitModule<TSelf, TParent> : UnitModule<TParent>
        where TSelf : UnitModule<TSelf, TParent>
        where TParent : class {


        // Fields.

        private readonly ParentPassport<TSelf> ownPassport = null;


        // Constructor.

        private protected UnitModule() : base() {
            ownPassport = new ParentPassport<TSelf>(this as TSelf);
        }


        // Properties.

        private protected ParentPassport<TSelf> OwnPassport => ownPassport;


    }


}

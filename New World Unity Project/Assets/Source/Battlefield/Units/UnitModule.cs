using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public abstract class UnitModule<TParent>
        where TParent : class {

        // Fields.

        private ParentPassport<TParent> parentPassport = null;


        // Properties.

        public TParent Parent => parentPassport.Parent;
        public bool Connected => !(Parent is null);


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

        private protected void ValidatePassport(ParentPassport<TParent> passport) {
            if (passport == null || passport != parentPassport) {
                throw new System.InvalidOperationException("Operation may be performed only by parent!");
            }
        }


    }

    public abstract class UnitModule<TSelf, TParent> : UnitModule<TParent>
        where TSelf : UnitModule<TSelf, TParent>
        where TParent : class {


        // Fields.

        private ParentPassport<TSelf> ownPassport = null;


        // Constructor.

        private protected UnitModule() : base() {
            ownPassport = new ParentPassport<TSelf>(this as TSelf);
        }


        // Properties.

        private protected ParentPassport<TSelf> OwnPassport => ownPassport;


    }


}

using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public class ParentPassport<TParent>
        where TParent : class {

        // Fields.

        private TParent parent;


        // Properties.

        public TParent Parent => parent;


        // Constructor.

        public ParentPassport(TParent parent) {
            if (parent is null) {
                throw new System.ArgumentNullException(nameof(parent));
            }
            this.parent = parent;
        }


    }


}

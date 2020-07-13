using System;
using UnityEngine.UIElements;

namespace NewWorld.Utilities {
    
    public class ClassWrapper<TWrapped>
        where TWrapped : class {

        // Fields.

        private readonly TWrapped wrapped;


        // Constructor.

        public ClassWrapper(TWrapped wrapped) {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }


        // Properties.

        private protected TWrapped Wrapped => wrapped;


    }

}

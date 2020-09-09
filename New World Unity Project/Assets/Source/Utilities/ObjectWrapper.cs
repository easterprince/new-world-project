using System;

namespace NewWorld.Utilities {

    public class ObjectWrapper<TWrapped>
        where TWrapped : class {

        // Fields.

        private readonly TWrapped wrapped;


        // Constructor.

        public ObjectWrapper(TWrapped wrapped) {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }


        // Properties.

        private protected TWrapped Wrapped => wrapped;


    }

}

using NewWorld.Utilities;

namespace NewWorld.Controllers.MetaData {

    public abstract class DescriptorBase {

        // Delegate.

        public delegate string Extractor<TSource>(TSource condition);


        // Fields.

        private readonly NamedId id;


        // Constructors.

        public DescriptorBase(NamedId id) {
            this.id = id;
        }


        // Properties.

        public NamedId Id => id;


    }

}

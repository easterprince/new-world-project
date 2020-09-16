using NewWorld.Utilities;

namespace NewWorld.Controllers.MetaData {
    
    public abstract class DescriptorBase {

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

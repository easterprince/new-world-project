using NewWorld.Utilities;

namespace NewWorld.Controllers.MetaData {

    public class AbilityDescriptor : DescriptorBase {

        // Static.

        private static readonly AbilityDescriptor defaultDescriptor = new AbilityDescriptor(NamedId.Default, null, null);

        public static AbilityDescriptor Default => defaultDescriptor;


        // Fields.

        private readonly string name;
        private readonly string description;


        // Constructor.

        public AbilityDescriptor(NamedId id, string name, string description) :
            base(id) {

            this.name = name ?? "Unknown ability";
            this.description = description ?? "Effect is unknown.";

        }


        // Properties.

        public string Name => name;
        public string Description => description;


    }

}

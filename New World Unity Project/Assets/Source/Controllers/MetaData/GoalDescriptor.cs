using NewWorld.Cores.Battle.Unit.Behaviours;
using NewWorld.Utilities;

namespace NewWorld.Controllers.MetaData {

    public class GoalDescriptor : DescriptorBase {

        // Fields.

        private readonly Extractor<UnitGoal> descriptionExtractor;

        // Static.
        private readonly static GoalDescriptor defaultDescriptor = new GoalDescriptor(NamedId.Default, null);


        // Constructor.

        public GoalDescriptor(NamedId id, Extractor<UnitGoal> descriptionExtractor) : base(id) {
            this.descriptionExtractor = descriptionExtractor ?? ((goal) => "Unknown goal");
        }


        // Properties.

        public static GoalDescriptor Default => defaultDescriptor;


        // Methods.

        public string ComposeDescription(UnitGoal goal) => descriptionExtractor.Invoke(goal);


    }

}

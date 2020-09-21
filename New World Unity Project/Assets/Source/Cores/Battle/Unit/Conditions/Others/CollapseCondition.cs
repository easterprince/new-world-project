using NewWorld.Cores.Battle.UnitSystem;
using NewWorld.Utilities;

namespace NewWorld.Cores.Battle.Unit.Conditions.Others {

    public class CollapseCondition :
        ConditionModuleBase<CollapseCondition, CollapseConditionPresentation>, ICollapseConditionPresentation {

        // Fields.

        // Meta.
        private NamedId id;

        // Progress.
        private float timeUntilExtinction;


        // Constructor.

        public CollapseCondition(NamedId id, float timeUntilExtinction = 1f) {
            this.id = id;
            this.timeUntilExtinction = Floats.SetPositive(timeUntilExtinction);
        }

        public CollapseCondition(CollapseCondition other) {
            id = other.id;
            timeUntilExtinction = other.timeUntilExtinction;
        }


        // Properties.

        public float TimeUntilExtinction => timeUntilExtinction;
        public override bool Cancellable => false;
        public override float ConditionSpeed => 1;
        public override NamedId Id => id;


        // Cloning.

        public override CollapseCondition Clone() {
            return new CollapseCondition(this);
        }


        // Presentation generation.

        private protected override CollapseConditionPresentation BuildPresentation() {
            return new CollapseConditionPresentation(this);
        }


        // Updating.

        private protected override void OnAct(out bool finished) {
            ValidateContext();
            finished = false;

            // Update time. 
            timeUntilExtinction = Floats.SetPositive(timeUntilExtinction - Context.GameTimeDelta);

            // Go extinct.
            if (timeUntilExtinction == 0f) {
                Context.UnitSystem.PlanAction(new UnitRemovalAction(Owner));
            }

        }


    }

}

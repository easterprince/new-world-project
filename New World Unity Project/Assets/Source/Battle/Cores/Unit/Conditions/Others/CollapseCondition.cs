using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Others {

    public class CollapseCondition :
        ConditionModuleBase<CollapseCondition, CollapseConditionPresentation>, ICollapseConditionPresentation {

        // Fields.

        // Meta.
        private ConditionId id;

        // Progress.
        private float timeUntilExtinction;


        // Constructor.

        public CollapseCondition(ConditionId id, float timeUntilExtinction = 1f) {
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
        public override ConditionId Id => id;
        public override string Description => $"Collapsing. Until extinction: {timeUntilExtinction}s.";


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

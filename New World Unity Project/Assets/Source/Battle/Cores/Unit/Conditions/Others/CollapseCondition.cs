using NewWorld.Battle.Cores.UnitSystem;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Others {

    public class CollapseCondition : ConditionModuleBase<CollapseCondition, CollapseConditionPresentation> {

        // Fields.

        private float timeUntilExtinction;


        // Constructor.

        public CollapseCondition(float timeUntilExtinction = 1f) {
            this.timeUntilExtinction = Mathf.Max(timeUntilExtinction, 0f);
        }

        public CollapseCondition(CollapseCondition other) {
            timeUntilExtinction = other.timeUntilExtinction;
        }


        // Properties.

        public float TimeUntilExtinction => timeUntilExtinction;

        public override bool Cancellable => false;

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
            timeUntilExtinction = Mathf.Max(timeUntilExtinction - Context.GameTimeDelta, 0f);
            
            // Go extinct.
            if (timeUntilExtinction == 0f) {
                Context.UnitSystem.PlanAction(new UnitRemovalAction(Owner));
            }

        }


    }

}

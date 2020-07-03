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

        public override string Description => "Collapsing.";


        // Cloning.

        public override CollapseCondition Clone() {
            return new CollapseCondition(this);
        }


        // Presentation generation.

        private protected override CollapseConditionPresentation BuildPresentation() {
            throw new System.NotImplementedException();
        }


        // Updating.

        public override void Update() {
            ValidateContext();
            timeUntilExtinction = Mathf.Max(timeUntilExtinction - Context.GameTimeDelta, 0f);
            if (timeUntilExtinction == 0f) {
                Context.UnitSystem.PlanAction(new UnitRemovalAction(Owner));
            }
        }


    }

}

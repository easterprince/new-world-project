using NewWorld.Battle.Cores.UnitSystem;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions {

    public class CollapsingCondition : ConditionModule {

        // Fields.

        private float timeUntilExtinction;


        // Constructor.

        public CollapsingCondition(float timeUntilExtinction = 1f) {
            this.timeUntilExtinction = Mathf.Max(timeUntilExtinction, 0f);
        }

        public CollapsingCondition(CollapsingCondition other) {
            timeUntilExtinction = other.timeUntilExtinction;
        }


        // Properties.

        public override string Description => "Collapsing.";


        // Cloning.

        public override ConditionModule Clone() {
            return new CollapsingCondition(this);
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

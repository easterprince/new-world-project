using System;

namespace NewWorld.Cores.Battle.Unit.Conditions {
    
    public class ConditionChangingAction : UnitAction {

        // Fields.

        private readonly IConditionModule condition;
        private readonly bool forceChange;


        // Constructor.

        public ConditionChangingAction(IConditionModule condition, bool forceChange) {
            this.condition = condition;
            this.forceChange = forceChange;
        }


        // Properties.

        public IConditionModule Condition => condition;
        public bool ForceChange => forceChange;

    
    }

}

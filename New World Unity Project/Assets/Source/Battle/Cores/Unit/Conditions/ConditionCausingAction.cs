using System;

namespace NewWorld.Battle.Cores.Unit.Conditions {
    
    public class ConditionCausingAction : UnitAction {

        // Fields.

        private readonly ConditionModule condition;


        // Constructor.

        public ConditionCausingAction(ConditionModule condition) {
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }


        // Properties.

        public ConditionModule Condition => condition;

    
    }

}

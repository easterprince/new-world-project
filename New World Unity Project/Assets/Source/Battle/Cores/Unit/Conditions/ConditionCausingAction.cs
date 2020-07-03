using System;

namespace NewWorld.Battle.Cores.Unit.Conditions {
    
    public class ConditionCausingAction : UnitAction {

        // Fields.

        private readonly IConditionModule condition;


        // Constructor.

        public ConditionCausingAction(IConditionModule condition) {
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }


        // Properties.

        public IConditionModule Condition => condition;

    
    }

}

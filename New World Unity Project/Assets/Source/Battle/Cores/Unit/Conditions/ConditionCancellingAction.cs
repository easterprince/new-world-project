using System;

namespace NewWorld.Battle.Cores.Unit.Conditions {
    
    public class ConditionCancellingAction : UnitAction {

        // Fields.

        private readonly IConditionPresentation condition;


        // Constructor.

        public ConditionCancellingAction(IConditionPresentation condition) {
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        // Properties.

        public IConditionPresentation Condition => condition;


    }

}

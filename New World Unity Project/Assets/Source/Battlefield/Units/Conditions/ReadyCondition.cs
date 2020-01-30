using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions {
    
    public struct ReadyCondition {

        // Fields.

        private readonly Condition condition;


        // Properties.

        public Condition Condition => condition;


        // Constructor.

        public ReadyCondition(Condition condition) {
            if (condition == null) {
                throw new System.ArgumentNullException(nameof(condition));
            }
            if (!condition.Ready) {
                throw new System.ArgumentException("Condition must be ready.");
            }
            this.condition = condition;
        }

    
    }

}

using UnityEngine;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class StopCondition : GeneralUnitUpdate {

        // Fields.

        private readonly Condition condition;
        private readonly bool forceStop;


        // Properties.

        public Condition Condition => condition;
        public bool ForceStop => forceStop;


        // Constructor.

        public StopCondition(Condition condition, bool forceStop) : base(condition.Owner) {
            this.condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            this.forceStop = forceStop;
        }


    }


}

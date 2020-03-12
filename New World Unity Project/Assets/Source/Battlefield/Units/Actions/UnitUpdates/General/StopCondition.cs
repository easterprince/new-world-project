using UnityEngine;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class StopCondition : GeneralUnitUpdate {

        // Fields.

        private readonly UnitCondition condition;
        private readonly bool forceStop;


        // Properties.

        public UnitCondition Condition => condition;
        public bool ForceStop => forceStop;


        // Constructor.

        public StopCondition(UnitCondition condition, bool forceStop) : base(condition.Owner) {
            this.condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            this.forceStop = forceStop;
        }


    }


}

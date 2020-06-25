using UnityEngine;
using NewWorld.Battlefield.Unit.Conditions;
using NewWorld.Battlefield.Unit.Actions.UnitUpdates.General;

namespace NewWorld.Battlefield.Unit.Core {

    public class ConditionStop : GeneralUnitUpdate {

        // Fields.

        private readonly UnitCondition condition;
        private readonly bool forceStop;


        // Properties.

        public UnitCondition Condition => condition;
        public bool ForceStop => forceStop;


        // Constructor.

        public ConditionStop(UnitCondition condition, bool forceStop) : base(condition.Owner) {
            this.condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            this.forceStop = forceStop;
        }


    }


}

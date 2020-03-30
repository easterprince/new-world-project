using UnityEngine;
using NewWorld.Battlefield.Unit.Conditions;
using NewWorld.Battlefield.Unit.Actions.UnitUpdates.General;

namespace NewWorld.Battlefield.Unit.Core {

    public class ConditionChange : GeneralUnitUpdate {

        // Fields.

        private readonly UnitCondition condition;


        // Properties.

        public UnitCondition Condition => condition;


        // Constructor.

        public ConditionChange(UnitController unit, UnitCondition condition) : base(unit) {
            this.condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
        }


    }


}

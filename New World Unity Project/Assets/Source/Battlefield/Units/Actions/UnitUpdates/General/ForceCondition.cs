using UnityEngine;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class ForceCondition : GeneralUnitUpdate {

        // Fields.

        private readonly UnitCondition condition;


        // Properties.

        public UnitCondition Condition => condition;


        // Constructor.

        public ForceCondition(UnitController unit, UnitCondition condition) : base(unit) {
            this.condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
        }


    }


}

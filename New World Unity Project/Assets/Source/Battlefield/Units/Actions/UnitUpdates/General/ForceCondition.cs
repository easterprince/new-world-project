using UnityEngine;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class ForceCondition : GeneralUnitUpdate {

        // Fields.

        private readonly ICondition condition;


        // Properties.

        public ICondition Condition => condition;


        // Constructor.

        public ForceCondition(UnitController unit, ICondition condition) : base(unit) {
            this.condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
        }


    }


}

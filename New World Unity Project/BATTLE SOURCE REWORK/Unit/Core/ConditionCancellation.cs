using UnityEngine;
using NewWorld.Battlefield.Unit.Conditions;

namespace NewWorld.Battlefield.Unit.Core {

    public class ConditionCancellation : ConditionStop {

        // Constructor.

        public ConditionCancellation(UnitCondition condition) : base(condition, false) { }


    }


}

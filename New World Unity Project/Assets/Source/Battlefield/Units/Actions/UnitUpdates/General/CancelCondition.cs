using UnityEngine;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class CancelCondition : StopCondition {

        // Constructor.

        public CancelCondition(Condition condition) : base(condition, false) {}


    }


}

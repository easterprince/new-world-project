using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;

namespace NewWorld.Battlefield.Units.Conditions {
    
    public interface ICondition : IUnitModule {

        // Properties.
        bool Exited { get; }
        bool CanBeCancelled { get; }
        new IConditionPresentation Presentation { get; }

        // Methods.
        IEnumerable<GameAction> Enter(UnitController owner);
        IEnumerable<GameAction> Update();
        IEnumerable<GameAction> Stop(bool forceStop);

    }

}

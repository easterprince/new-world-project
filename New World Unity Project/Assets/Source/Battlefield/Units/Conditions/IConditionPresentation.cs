using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions {
    
    public interface IConditionPresentation : IUnitModulePresentation {

        // Properties.
        bool Exited { get; }
        bool CanBeCancelled { get; }

    }

}

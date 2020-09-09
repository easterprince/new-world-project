using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Motions {
    
    public interface IMotionConditionPresentation : IConditionPresentation {
        
        // Properties.

        Vector3 Destination { get; }
        float MovementPerSecond { get; }


    }

}

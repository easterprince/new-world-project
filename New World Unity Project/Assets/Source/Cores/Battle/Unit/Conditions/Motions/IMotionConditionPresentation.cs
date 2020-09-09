using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Conditions.Motions {
    
    public interface IMotionConditionPresentation : IConditionPresentation {
        
        // Properties.

        Vector3 Destination { get; }
        float MovementPerSecond { get; }


    }

}

using NewWorld.Battle.Cores.Unit;
using UnityEngine;

namespace NewWorld.Battle.Cores.UnitSystem {

    public class UnitSystemPresentation : PresentationBase<UnitSystemCore>,
        IReceptive<UnitAdditionAction>, IReceptive<UnitMotionAction>, IReceptive<UnitRemovalAction> {
        
        // Constructor.

        public UnitSystemPresentation(UnitSystemCore presented) : base(presented) {}


        // Properties.

        public Vector2Int this[UnitCore unit] => Presented[unit];
        public UnitCore this[Vector2Int position] => Presented[position];
        public bool HasUnit(UnitCore unit) => Presented.HasUnit(unit);


        // Action planning.

        public void PlanAction(UnitAdditionAction action) => Presented.PlanAction(action);
        public void PlanAction(UnitMotionAction action) => Presented.PlanAction(action);
        public void PlanAction(UnitRemovalAction action) => Presented.PlanAction(action);


    }

}
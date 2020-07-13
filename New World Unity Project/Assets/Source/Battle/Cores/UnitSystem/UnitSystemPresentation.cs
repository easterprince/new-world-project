using NewWorld.Battle.Cores.Unit;
using NewWorld.Utilities.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Cores.UnitSystem {

    public class UnitSystemPresentation : PresentationBase<UnitSystemCore>, IEnumerable<UnitPresentation>,
        IReceptive<UnitAdditionAction>, IReceptive<UnitMotionAction>, IReceptive<UnitRemovalAction> {
        
        // Constructor.

        public UnitSystemPresentation(UnitSystemCore presented) : base(presented) {}


        // Properties.

        public Vector2Int this[UnitPresentation unitPresentation] => Presented[unitPresentation];
        public UnitPresentation this[Vector2Int position] => Presented[position];

        public GameEvent<UnitPresentation>.EventWrapper AdditionEvent => Presented.AdditionEvent;
        public GameEvent<UnitPresentation>.EventWrapper RemovalEvent => Presented.RemovalEvent;


        // Enumeration.

        public IEnumerator<UnitPresentation> GetEnumerator() {
            return Presented.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }


        // Methods.

        public bool HasUnit(UnitPresentation unitPresentation) => Presented.HasUnit(unitPresentation);


        // Action planning.

        public void PlanAction(UnitAdditionAction action) => Presented.PlanAction(action);
        public void PlanAction(UnitMotionAction action) => Presented.PlanAction(action);
        public void PlanAction(UnitRemovalAction action) => Presented.PlanAction(action);


    }

}
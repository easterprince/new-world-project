using NewWorld.Cores.Battle.Unit;
using NewWorld.Utilities;
using NewWorld.Utilities.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Cores.Battle.UnitSystem {

    public class UnitSystemPresentation : PresentationBase<UnitSystemCore>, IEnumerable<UnitPresentation>,
        IReceptive<UnitAdditionAction>, IReceptive<UnitMotionAction>, IReceptive<UnitRemovalAction> {
        
        // Constructor.

        public UnitSystemPresentation(UnitSystemCore presented) : base(presented) {}


        // Properties.

        public Vector2Int this[UnitPresentation unitPresentation] => Presented[unitPresentation];
        public UnitPresentation this[Vector2Int position] => Presented[position];

        public ObjectState<UnitPresentation>.StateWrapper State => Presented.State;



        // Enumeration.

        public IEnumerator<UnitPresentation> GetEnumerator() {
            return Presented.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }


        // Methods.

        public bool Contains(UnitPresentation unitPresentation) => Presented.Contains(unitPresentation);


        // Action planning.

        public void PlanAction(UnitAdditionAction action) => Presented.PlanAction(action);
        public void PlanAction(UnitMotionAction action) => Presented.PlanAction(action);
        public void PlanAction(UnitRemovalAction action) => Presented.PlanAction(action);


    }

}
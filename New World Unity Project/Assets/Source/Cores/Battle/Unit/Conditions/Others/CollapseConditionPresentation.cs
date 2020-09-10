﻿namespace NewWorld.Cores.Battle.Unit.Conditions.Others {

    public class CollapseConditionPresentation :
        ConditionPresentationBase<ICollapseConditionPresentation> {

        // Constructor.

        public CollapseConditionPresentation(ICollapseConditionPresentation presented) : base(presented) {}


        // Properties.

        public float TimeUntilExtinction => Presented.TimeUntilExtinction;


    }

}
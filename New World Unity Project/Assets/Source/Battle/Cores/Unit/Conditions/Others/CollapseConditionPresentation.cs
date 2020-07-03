namespace NewWorld.Battle.Cores.Unit.Conditions.Others {

    public class CollapseConditionPresentation : ConditionPresentationBase<CollapseCondition> {

        // Constructor.

        public CollapseConditionPresentation(CollapseCondition presented) : base(presented) {}


        // Properties.

        public float TimeUntilExtinction => Presented.TimeUntilExtinction;


    }

}

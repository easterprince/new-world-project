namespace NewWorld.Battle.Cores.Unit.Behaviours {

    public class BehaviourPresentationBase<TSelf, TPresented, TGoal> : UnitModulePresentationBase<TPresented>, IBehaviourPresentation
        where TSelf : BehaviourPresentationBase<TSelf, TPresented, TGoal>
        where TPresented : BehaviourModuleBase<TPresented, TSelf, TGoal>
        where TGoal : UnitGoal {

        // Constructor.

        public BehaviourPresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public TGoal Goal => Presented.Goal;

        UnitGoal IBehaviourPresentation.Goal => Presented.Goal;


    }

}

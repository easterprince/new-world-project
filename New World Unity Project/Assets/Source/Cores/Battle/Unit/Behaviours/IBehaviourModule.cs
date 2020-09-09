namespace NewWorld.Cores.Battle.Unit.Behaviours {

    public interface IBehaviourModule :
        IUnitModule<IBehaviourModule, IBehaviourPresentation, IOwnerPointer>, IBehaviourPresentation {

        void Act(out GoalStatus goalStatus);


    }

}

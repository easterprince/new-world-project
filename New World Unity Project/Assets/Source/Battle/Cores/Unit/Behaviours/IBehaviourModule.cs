using NewWorld.Battle.Cores.Unit.Intelligence;

namespace NewWorld.Battle.Cores.Unit.Behaviours {
    
    public interface IBehaviourModule :
        IUnitModule<IBehaviourModule, IBehaviourPresentation, IOwnerPointer>, IBehaviourPresentation {

        void Act(out GoalStatus goalStatus);


    }

}

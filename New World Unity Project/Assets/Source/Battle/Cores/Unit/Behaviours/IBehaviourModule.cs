using NewWorld.Battle.Cores.Unit.Intelligence;

namespace NewWorld.Battle.Cores.Unit.Behaviours {
    
    public interface IBehaviourModule :
        IUnitModule<IBehaviourModule, IBehaviourPresentation, IntelligencePresentation>, IBehaviourPresentation {

        void Act(out GoalStatus goalStatus);


    }

}

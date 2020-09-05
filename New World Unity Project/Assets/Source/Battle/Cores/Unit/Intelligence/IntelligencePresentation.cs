using NewWorld.Battle.Cores.Unit.Behaviours;

namespace NewWorld.Battle.Cores.Unit.Intelligence {

    public class IntelligencePresentation : UnitModulePresentationBase<IntelligenceModule> {
        
        // Constructor.
        
        public IntelligencePresentation(IntelligenceModule presented) : base(presented) {}


        // Properties.

        public UnitGoal CurrentGoal => Presented.CurrentGoal;


    }

}
using NewWorld.Cores.Battle.Unit.Behaviours;

namespace NewWorld.Cores.Battle.Unit.Intelligence {

    public class IntelligencePresentation : UnitModulePresentationBase<IntelligenceModule> {

        // Constructor.

        public IntelligencePresentation(IntelligenceModule presented) : base(presented) {}


        // Properties.

        public UnitGoal CurrentGoal => Presented.CurrentGoal;


    }

}
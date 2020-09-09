using NewWorld.Cores.Battle.Unit.Abilities;
using NewWorld.Cores.Battle.Unit.Behaviours;
using NewWorld.Cores.Battle.Unit.Behaviours.Offensives;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using NewWorld.Cores.Battle.Unit.Conditions;
using System.Collections.Generic;

namespace NewWorld.Cores.Battle.Unit.Intelligence {

    public class IntelligenceModule : UnitModuleBase<IntelligenceModule, IntelligencePresentation, UnitPresentation> {

        // Fields.

        private IBehaviourModule currentBehaviour = null;


        // Constructors.

        public IntelligenceModule() {}

        public IntelligenceModule(IntelligenceModule other) {
            if (other.currentBehaviour != null) {
                currentBehaviour = other.currentBehaviour.Clone();
                currentBehaviour.Connect(Presentation);
            }
        }


        // Properties.

        public UnitGoal CurrentGoal => currentBehaviour?.Goal ?? IdleGoal.Instance;


        // Cloning.
        
        public override IntelligenceModule Clone() {
            return new IntelligenceModule(this);
        }


        // Presentation generation.

        private protected override IntelligencePresentation BuildPresentation() {
            return new IntelligencePresentation(this);
        }


        // Methods.

        public void Act() {
            if (currentBehaviour != null) {
                currentBehaviour.Act(out var goalStatus);
                if (goalStatus != GoalStatus.Active) {
                    currentBehaviour = null;
                }
            }
        }

        public void SetGoal(RelocationGoal goal) {
            currentBehaviour?.Disconnect();
            currentBehaviour = new RelocationBehaviour {
                Goal = goal
            };
            currentBehaviour.Connect(Presentation);
        }

        public void SetGoal(OffensiveGoal goal) {
            currentBehaviour?.Disconnect();
            currentBehaviour = new OffensiveBehaviour {
                Goal = goal
            };
            currentBehaviour.Connect(Presentation);
        }


    }

}

using NewWorld.Battle.Cores.Unit.Abilities;
using NewWorld.Battle.Cores.Unit.Behaviours;
using NewWorld.Battle.Cores.Unit.Behaviours.Relocations;
using NewWorld.Battle.Cores.Unit.Conditions;
using System.Collections.Generic;

namespace NewWorld.Battle.Cores.Unit.Intelligence {

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
            var newBehaviour = new RelocationBehaviour {
                Goal = goal
            };
            currentBehaviour?.Disconnect();
            currentBehaviour = newBehaviour;
            currentBehaviour.Connect(Presentation);
        }


    }

}

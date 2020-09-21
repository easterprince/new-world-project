using NewWorld.Cores.Battle.Unit.Behaviours;
using NewWorld.Cores.Battle.Unit.Behaviours.Offensives;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using System;

namespace NewWorld.Cores.Battle.Unit.Intelligence {

    public class IntelligenceModule : UnitModuleBase<IntelligenceModule, IntelligencePresentation, UnitPresentation> {

        // Fields.

        private IBehaviour currentBehaviour = null;


        // Constructors.

        public IntelligenceModule() {}

        public IntelligenceModule(IntelligenceModule other) {
            if (other is null) {
                throw new ArgumentNullException(nameof(other));
            }

            // Set behaviour.
            var otherGoal = other?.currentBehaviour?.Goal;
            if (otherGoal is OffensiveGoal offensiveGoal) {
                SetGoal(offensiveGoal);
            } else if (otherGoal is RelocationGoal relocationGoal) {
                SetGoal(relocationGoal);
            } else {
                SetGoal(IdleGoal.Instance);
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
            currentBehaviour = new RelocationBehaviour(goal, this);
        }

        public void SetGoal(OffensiveGoal goal) {
            currentBehaviour = new OffensiveBehaviour(goal, this);
        }

        public void SetGoal(IdleGoal goal) {
            currentBehaviour = null;
        }


    }

}

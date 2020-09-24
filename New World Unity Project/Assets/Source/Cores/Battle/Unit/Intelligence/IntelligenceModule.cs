using NewWorld.Cores.Battle.Unit.Behaviours;
using NewWorld.Cores.Battle.Unit.Behaviours.Offensives;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using System;
using System.Collections.Generic;

namespace NewWorld.Cores.Battle.Unit.Intelligence {

    public class IntelligenceModule : UnitModuleBase<IntelligenceModule, IntelligencePresentation, UnitPresentation> {

        // Delegate.

        private delegate IBehaviour BehaviourGenerator(UnitGoal goal, IOwnerPointer ownerPointer);


        // Fields.

        private readonly Dictionary<Type, BehaviourGenerator> behaviourGenerators;
        private IBehaviour currentBehaviour;


        // Constructors.

        public IntelligenceModule() {
            
            // Set fields.
            behaviourGenerators = new Dictionary<Type, BehaviourGenerator>() {
                [typeof(RelocationGoal)] = (goal, ownerPointer) => new RelocationBehaviour(goal as RelocationGoal, ownerPointer),
                [typeof(OffensiveGoal)] = (goal, ownerPointer) => new OffensiveBehaviour(goal as OffensiveGoal, ownerPointer)
            };
            currentBehaviour = null;

        }

        public IntelligenceModule(IntelligenceModule other) {
            if (other is null) {
                throw new ArgumentNullException(nameof(other));
            }

            // Set fields.
            behaviourGenerators = new Dictionary<Type, BehaviourGenerator>(other.behaviourGenerators);
            SetGoal(other.CurrentGoal);

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


        // Public methods.

        public void Act() {
            if (currentBehaviour != null) {
                currentBehaviour.Act(out var goalStatus);
                if (goalStatus != GoalStatus.Active) {
                    currentBehaviour = null;
                }
            }
        }

        public void SetGoal(UnitGoal goal) {
            if (behaviourGenerators.TryGetValue(goal.GetType(), out var generator)) {
                currentBehaviour = generator.Invoke(goal, this);
            } else {
                currentBehaviour = null;
            }
        }


    }

}

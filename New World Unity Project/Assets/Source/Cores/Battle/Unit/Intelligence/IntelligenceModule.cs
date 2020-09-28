using NewWorld.Cores.Battle.Unit.Behaviours;
using NewWorld.Cores.Battle.Unit.Behaviours.Offensives;
using NewWorld.Cores.Battle.Unit.Behaviours.Relocations;
using NewWorld.Utilities;
using System;
using System.Collections.Generic;

namespace NewWorld.Cores.Battle.Unit.Intelligence {

    public class IntelligenceModule : UnitModuleBase<IntelligenceModule, IntelligencePresentation, UnitPresentation> {

        // Delegate.

        private delegate IBehaviour BehaviourGenerator(UnitGoal goal, IOwnerPointer ownerPointer);


        // Fields.

        private readonly Dictionary<Type, BehaviourGenerator> behaviourGenerators;
        private TimeAccumulator rethinkTimer;
        private IBehaviour currentBehaviour;


        // Constructors.

        public IntelligenceModule() {
            
            // Set fields.
            behaviourGenerators = new Dictionary<Type, BehaviourGenerator>() {
                [typeof(RelocationGoal)] = (goal, ownerPointer) => new RelocationBehaviour(goal as RelocationGoal, ownerPointer),
                [typeof(OffensiveGoal)] = (goal, ownerPointer) => new OffensiveBehaviour(goal as OffensiveGoal, ownerPointer)
            };
            rethinkTimer = new TimeAccumulator(1);
            currentBehaviour = null;

        }

        public IntelligenceModule(IntelligenceModule other) {
            if (other is null) {
                throw new ArgumentNullException(nameof(other));
            }

            // Set fields.
            behaviourGenerators = new Dictionary<Type, BehaviourGenerator>(other.behaviourGenerators);
            rethinkTimer = new TimeAccumulator(other.rethinkTimer);
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

            // Rethink if needed.
            if (currentBehaviour != null && Context != null) {
                rethinkTimer.Add(Context.GameTimeDelta, out bool doRethink);
                if (doRethink) {
                    SetGoal(currentBehaviour.Goal);
                }
            }

            // Let behaviour act.
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
            rethinkTimer.Reset();
        }


    }

}

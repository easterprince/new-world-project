using NewWorld.Cores.Battle.Unit.Intelligence;

namespace NewWorld.Cores.Battle.Unit.Behaviours {
    
    public abstract class BehaviourModuleBase<TSelf, TPresentation, TGoal> :
        UnitModuleBase<TSelf, TPresentation, IOwnerPointer>, IBehaviourModule
        where TSelf : BehaviourModuleBase<TSelf, TPresentation, TGoal>
        where TPresentation : BehaviourPresentationBase<TPresentation, TSelf, TGoal>
        where TGoal : UnitGoal {

        // Fields.

        private TGoal goal = null;


        // Properties.

        public TGoal Goal {
            get => goal;
            set => goal = value;
        }

        UnitGoal IBehaviourPresentation.Goal => goal;

        IBehaviourPresentation ICore<IBehaviourModule, IBehaviourPresentation>.Presentation => Presentation;


        // Cloning.

        public sealed override TSelf Clone() {
            var partialClone = ClonePartially();
            partialClone.Goal = Goal;
            return partialClone;
        }

        private protected abstract TSelf ClonePartially();

        IBehaviourModule ICore<IBehaviourModule, IBehaviourPresentation>.Clone() => Clone();


        // Methods.

        public void Act(out GoalStatus goalStatus) {
            ValidateContext();
            if (goal is null) {
                goalStatus = GoalStatus.Achieved;
                return;
            }
            OnAct(out goalStatus);
            if (goalStatus != GoalStatus.Active) {
                goal = null;
            }
        }

        private protected abstract void OnAct(out GoalStatus goalStatus); 


    }

}

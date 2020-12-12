using NewWorld.Cores.Battle.Battlefield;
using System;

namespace NewWorld.Cores.Battle.Unit.Behaviours {

    public abstract class BehaviourBase<TGoal> : IBehaviour
        where TGoal : UnitGoal {

        // Fields.

        private TGoal goal = null;
        private readonly IOwnerPointer ownerPointer = null;


        // Constructors.

        public BehaviourBase(TGoal goal, IOwnerPointer ownerPointer) {
            this.goal = goal;
            this.ownerPointer = ownerPointer;
        }


        // Properties.

        public TGoal Goal => goal;
        UnitGoal IBehaviour.Goal => goal;
        public UnitPresentation Owner => ownerPointer?.Owner;
        public BattlefieldPresentation Context => ownerPointer?.Context;


        // Methods.

        public void Act(out GoalStatus goalStatus) {
            if (goal is null) {
                goalStatus = GoalStatus.Achieved;
                return;
            }
            OnAct(out goalStatus);
            if (goalStatus != GoalStatus.Active) {
                goal = null;
                if (goalStatus != GoalStatus.Achieved && goalStatus != GoalStatus.Failed) {
                    goalStatus = GoalStatus.Failed;
                }
            }
        }

        private protected abstract void OnAct(out GoalStatus goalStatus);


    }

}

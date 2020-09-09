using System;

namespace NewWorld.Cores.Battle.Unit.Behaviours {

    public class GoalSettingAction<TGoal> : UnitAction
        where TGoal : UnitGoal {

        // Fields.

        private readonly TGoal goal;


        // Constructor.

        public GoalSettingAction(TGoal goal) {
            this.goal = goal ?? throw new ArgumentNullException(nameof(goal));
        }


        // Properties.

        public TGoal Goal => goal;


    }

}

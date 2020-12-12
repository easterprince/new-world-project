using System;

namespace NewWorld.Cores.Battle.Unit.Behaviours {

    public class GoalSettingAction : UnitAction {

        // Fields.

        private readonly UnitGoal goal;


        // Constructor.

        public GoalSettingAction(UnitGoal goal) {
            this.goal = goal ?? throw new ArgumentNullException(nameof(goal));
        }


        // Properties.

        public UnitGoal Goal => goal;


    }

}

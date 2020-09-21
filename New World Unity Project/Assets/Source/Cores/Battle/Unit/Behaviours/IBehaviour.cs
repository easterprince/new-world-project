namespace NewWorld.Cores.Battle.Unit.Behaviours {

    public interface IBehaviour : IOwnerPointer {

        // Properties.

        UnitGoal Goal { get; }


        // Methods.

        void Act(out GoalStatus goalStatus);


    }

}

namespace NewWorld.Battle.Cores {
    
    public interface IReceptive<TGameAction>
        where TGameAction : GameAction {

        void PlanAction(TGameAction action);


    }

}

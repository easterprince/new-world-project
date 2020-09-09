namespace NewWorld.Cores.Battle {
    
    public interface IReceptive<TGameAction>
        where TGameAction : GameAction {

        void PlanAction(TGameAction action);


    }

}

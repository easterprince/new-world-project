namespace NewWorld.Cores.Battle {
    
    public interface IResponsive<TGameAction> : IReceptive<TGameAction>
        where TGameAction : GameAction {

        void ProcessAction(TGameAction action);


    }

}

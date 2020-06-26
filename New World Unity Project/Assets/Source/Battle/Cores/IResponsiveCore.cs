namespace NewWorld.Battle.Cores {
    
    public interface IResponsiveCore<TGameAction> : ICore
        where TGameAction : GameAction {

        void ProcessAction(TGameAction action);


    }

}

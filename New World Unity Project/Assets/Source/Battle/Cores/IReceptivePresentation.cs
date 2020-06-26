namespace NewWorld.Battle.Cores {

    public interface IReceptivePresentation<TGameActionSeries> : IPresentation
        where TGameActionSeries : GameAction {

        void AddAction<TGameAction>(TGameAction action)
            where TGameAction : TGameActionSeries;


    }

}

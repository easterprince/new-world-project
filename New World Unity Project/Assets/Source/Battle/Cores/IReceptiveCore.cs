namespace NewWorld.Battle.Cores {

    public interface IReceptiveCore<TGameActionSeries> : ICore
        where TGameActionSeries : GameAction {

        IReceptivePresentation<TGameActionSeries> ReceptivePresentation { get; }

        void AddAction<TGameAction>(TGameAction action)
            where TGameAction : TGameActionSeries;


    }

}

namespace NewWorld.Battle.Cores {
    
    public interface IReceptiveCore : ICore {

        IReceptivePresentation ReceptivePresentation { get; }

        void ProcessAction(IGameAction action);

        void AddAction(IGameAction action);


    }

}

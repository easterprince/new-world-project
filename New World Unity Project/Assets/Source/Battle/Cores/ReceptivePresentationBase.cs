using NewWorld.Battle.Cores.Battlefield;

namespace NewWorld.Battle.Cores {

    public abstract class ReceptivePresentationBase<TPresented, TGameActionSeries> : PresentationBase<TPresented>, IReceptivePresentation<TGameActionSeries>
        where TPresented : class, IReceptiveCore<TGameActionSeries>
        where TGameActionSeries : GameAction {

        // Constructor.
        
        public ReceptivePresentationBase(TPresented presented) : base(presented) {}


        // Methods.

        public void AddAction<TAction>(TAction action)
            where TAction : TGameActionSeries {

            Presented.AddAction(action);
        }


    }

}

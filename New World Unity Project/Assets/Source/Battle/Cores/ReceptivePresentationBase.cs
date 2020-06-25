using NewWorld.Battle.Cores.Battlefield;

namespace NewWorld.Battle.Cores {

    public class ReceptivePresentationBase<TSelf, TPresented> : PresentationBase<TPresented>, IReceptivePresentation
        where TSelf : ReceptivePresentationBase<TSelf, TPresented>
        where TPresented : ReceptiveCoreBase<TPresented, TSelf> {

        // Constructor.
        
        public ReceptivePresentationBase(TPresented presented) : base(presented) {}


        // Methods.

        public void AddAction(IGameAction action) {
            Presented.AddAction(action);
        }


    }

}

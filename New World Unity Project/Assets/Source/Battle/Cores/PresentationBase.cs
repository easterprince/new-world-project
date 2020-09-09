using NewWorld.Battle.Cores.Battlefield;

namespace NewWorld.Battle.Cores {
    
    public abstract class PresentationBase<TPresented> : IContextPointer
        where TPresented : IContextPointer {

        // Fields.

        private readonly TPresented presented;


        // Properties.

        protected private TPresented Presented => presented;
        public BattlefieldPresentation Context => presented.Context;


        // Constructor.

        public PresentationBase(TPresented presented) {
            if (presented is null) {
                throw new System.ArgumentNullException(nameof(presented));
            }
            this.presented = presented;
        }


    }

}

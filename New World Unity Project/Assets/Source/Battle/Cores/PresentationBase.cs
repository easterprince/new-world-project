namespace NewWorld.Battle.Cores {
    
    public class PresentationBase<TPresented> : IPresentation
        where TPresented : class {

        // Fields.

        private TPresented presented;


        // Properties.

        protected private TPresented Presented => presented;


        // Constructor.

        public PresentationBase(TPresented presented) {
            if (presented is null) {
                throw new System.ArgumentNullException(nameof(presented))
            }
            this.presented = presented;
        }


    }

}

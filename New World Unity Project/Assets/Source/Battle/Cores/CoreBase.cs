namespace NewWorld.Battle.Cores {
    
    public abstract class CoreBase<TSelf, TPresentation> : ICore
        where TSelf : CoreBase<TSelf, TPresentation>
        where TPresentation : PresentationBase<TSelf> {

        // Fields.

        private TPresentation presentation;


        // Properties.

        public TPresentation Presentation {
            get {
                if (presentation is null) {
                    presentation = BuildPresentation();
                    if (presentation is null) {
                        throw new System.NullReferenceException("Presentation building method returned null reference.");
                    }
                }
                return presentation;
            }
        }

        IPresentation ICore.Presentation => Presentation;


        // Methods.

        private protected abstract TPresentation BuildPresentation();


    }

}

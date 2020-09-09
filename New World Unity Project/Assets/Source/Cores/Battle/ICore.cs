namespace NewWorld.Cores.Battle {
    
    public interface ICore<TSelf, TPresentation> : IContextPointer {

        // Properties.

        TPresentation Presentation { get; }


        // Methods.

        TSelf Clone();


    }

}


﻿namespace NewWorld.Battle.Cores {
    
    public interface ICore<TSelf, TPresentation> : IContextPointer {

        // Properties.

        TPresentation Presentation { get; }


        // Methods.

        TSelf Clone();


    }

}


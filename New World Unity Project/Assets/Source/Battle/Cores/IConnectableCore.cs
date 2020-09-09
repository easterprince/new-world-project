﻿using NewWorld.Battle.Cores.Battlefield;

namespace NewWorld.Battle.Cores {
    
    public interface IConnectableCore<TSelf, TPresentation, TParentPresentation> : ICore<TSelf, TPresentation>
        where TPresentation : class, IContextPointer
        where TParentPresentation : class, IContextPointer {

        // Properties.

        TParentPresentation Parent { get; }        
        bool Connected { get; }


        // Methods.

        void Connect(TParentPresentation parent);
        void Disconnect();


    }

}

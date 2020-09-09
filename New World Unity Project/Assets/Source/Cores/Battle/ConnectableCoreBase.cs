using NewWorld.Cores.Battle.Battlefield;
using System;

namespace NewWorld.Cores.Battle {

    public abstract class ConnectableCoreBase<TSelf, TPresentation, TParentPresentation> :
        CoreBase<TSelf, TPresentation>, IConnectableCore<TSelf, TPresentation, TParentPresentation>
        where TPresentation : class, IContextPointer
        where TParentPresentation : class, IContextPointer {

        // Fields.

        private TParentPresentation parent;


        // Properties.

        public TParentPresentation Parent => parent;
        public bool Connected => !(parent is null);
        public sealed override BattlefieldPresentation Context => parent?.Context;


        // Connection management.

        public void Connect(TParentPresentation parent) {
            if (Connected) {
                if (this.parent != parent) {
                    throw new InvalidOperationException("Module is already connected!");
                }
                return;
            }
            this.parent = parent;
        }

        public void Disconnect() {
            if (!Connected) {
                return;
            }
            parent = null;
        }


        // Support methods.

        private protected void ValidateConnection() {
            if (!Connected) {
                throw new NullReferenceException("Must be connected.");
            }
        }


    }

}

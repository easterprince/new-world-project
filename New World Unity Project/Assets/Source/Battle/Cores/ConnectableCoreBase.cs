using NewWorld.Battle.Cores.Battlefield;
using System;

namespace NewWorld.Battle.Cores {
    
    public abstract class ConnectableCoreBase<TPresentation, TParentPresentation> : CoreBase<TPresentation>
        where TPresentation : IContextPointer
        where TParentPresentation : class, IContextPointer {

        // Fields.

        private TParentPresentation parent;


        // Constructor.

        public ConnectableCoreBase(TParentPresentation parent) {
            if (parent is null) {
                throw new ArgumentNullException(nameof(parent));
            }
            this.parent = parent;
        }


        // Properties.

        public TParentPresentation Parent => parent;
        public bool Connected => !(parent is null);
        public override BattlefieldPresentation Context => parent?.Context;


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

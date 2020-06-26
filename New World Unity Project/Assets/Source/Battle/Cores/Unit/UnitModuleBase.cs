using System;

namespace NewWorld.Battle.Cores.Unit {
    
    public abstract class UnitModuleBase<TSelf, TPresentation, TParentPresentation> : CoreBase<TSelf, TPresentation>
        where TSelf : UnitModuleBase<TSelf, TPresentation, TParentPresentation>
        where TPresentation : PresentationBase<TSelf>
        where TParentPresentation : class, IParentPresentation {

        // Fields.

        private TParentPresentation parent;


        // Properties.

        public TParentPresentation Parent => parent;
        public bool Connected => !(parent is null);
        public UnitPresentation Owner => parent?.Owner;


        // Methods.

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


    }

}

using System;

namespace NewWorld.Battle.Cores.Unit {

    public abstract class UnitModuleCoreBase<TPresentation, TParentPresentation> : ConnectableCoreBase<TPresentation, TParentPresentation>, IOwnerPointer
        where TPresentation : IContextPointer
        where TParentPresentation : class, IOwnerPointer {
    
        // Properties.

        public UnitPresentation Owner => Parent?.Owner;


        // Support methods.

        private protected void ValidateOwnership() {
            if (Owner is null) {
                throw new NullReferenceException("Must be connected.");
            }
        }


    }

}

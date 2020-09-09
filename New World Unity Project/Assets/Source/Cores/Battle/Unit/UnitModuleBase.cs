using System;

namespace NewWorld.Cores.Battle.Unit {

    public abstract class UnitModuleBase<TSelf, TPresentation, TParentPresentation> :
        ConnectableCoreBase<TSelf, TPresentation, TParentPresentation>, IOwnerPointer,
        IUnitModule<TSelf, TPresentation, TParentPresentation>
        where TPresentation : class, IOwnerPointer
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

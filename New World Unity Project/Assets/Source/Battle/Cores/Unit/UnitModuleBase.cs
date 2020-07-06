using System;

namespace NewWorld.Battle.Cores.Unit {

    public abstract class UnitModuleBase<TSelf, TPresentation, TParentPresentation> :
        ConnectableCoreBase<TSelf, TPresentation, TParentPresentation>, IOwnerPointer,
        IUnitModule<TSelf, TPresentation, TParentPresentation>
        where TSelf : UnitModuleBase<TSelf, TPresentation, TParentPresentation>
        where TPresentation : class, IContextPointer
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

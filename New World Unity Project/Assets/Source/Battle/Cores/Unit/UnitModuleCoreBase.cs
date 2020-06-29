﻿using System;

namespace NewWorld.Battle.Cores.Unit {

    public abstract class UnitModuleCoreBase<TSelf, TPresentation, TParentPresentation> :
        ConnectableCoreBase<TSelf, TPresentation, TParentPresentation>, IOwnerPointer
        where TSelf : UnitModuleCoreBase<TSelf, TPresentation, TParentPresentation>
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

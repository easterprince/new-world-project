﻿namespace NewWorld.Battle.Cores.Unit {
    
    public abstract class UnitModulePresentationBase<TPresented> : PresentationBase<TPresented>, IOwnerPointer
        where TPresented : IOwnerPointer {
        
        // Constructor.
        
        public UnitModulePresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public UnitPresentation Owner => Presented.Owner;


    }

}
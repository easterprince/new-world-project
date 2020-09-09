namespace NewWorld.Cores.Battle.Unit {
    
    public abstract class UnitModulePresentationBase<TPresented> : PresentationBase<TPresented>, IOwnerPointer
        where TPresented : class, IOwnerPointer {
        
        // Constructor.
        
        public UnitModulePresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public UnitPresentation Owner => Presented.Owner;


    }

}

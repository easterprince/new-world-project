namespace NewWorld.Battle.Cores.Unit {
    
    public abstract class UnitPresentationBase<TPresented> : PresentationBase<TPresented>, IOwnerPointer
        where TPresented : IOwnerPointer {
        
        // Constructor.
        
        public UnitPresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public UnitPresentation Owner => Presented.Owner;


    }

}

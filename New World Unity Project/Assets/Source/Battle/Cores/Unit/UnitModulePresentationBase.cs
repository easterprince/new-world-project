namespace NewWorld.Battle.Cores.Unit {
    
    public abstract class UnitModulePresentationBase<TPresented> : PresentationBase<TPresented>, IParentPresentation
        where TPresented : class, IUnitModule {
        
        // Constructor.
        
        public UnitModulePresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public UnitPresentation Owner => Presented.Owner;


    }

}

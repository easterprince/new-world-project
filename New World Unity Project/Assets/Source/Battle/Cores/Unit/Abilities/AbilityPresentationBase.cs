namespace NewWorld.Battle.Cores.Unit.Abilities {

    public abstract class AbilityPresentationBase<TPresented> : UnitModulePresentationBase<TPresented>, IAbilityPresentation
        where TPresented : IAbilityPresentation {
        
        // Constructor.
        
        public AbilityPresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public string Description => Presented.Description;


    }

}
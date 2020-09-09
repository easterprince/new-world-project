namespace NewWorld.Cores.Battle.Unit.Abilities {

    public abstract class AbilityPresentationBase<TPresented> :
        UnitModulePresentationBase<TPresented>, IAbilityPresentation
        where TPresented : class, IAbilityPresentation {

        // Constructor.

        public AbilityPresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public string Name => Presented.Name;
        public string Description => Presented.Description;


    }

}
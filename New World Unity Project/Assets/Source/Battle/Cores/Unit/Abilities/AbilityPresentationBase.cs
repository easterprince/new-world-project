namespace NewWorld.Battle.Cores.Unit.Abilities {

    public abstract class AbilityPresentationBase<TPresented> : UnitPresentationBase<TPresented>, IAbility
        where TPresented : IOwnerPointer, IAbility {
        
        // Constructor.
        
        public AbilityPresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public string Description => Presented.Description;


    }

}
namespace NewWorld.Battle.Cores.Unit.Conditions {

    public class ConditionPresentationBase<TPresented> : UnitModulePresentationBase<TPresented>, IConditionPresentation
        where TPresented : IConditionModule, IOwnerPointer {
        
        // Constructor.
        
        public ConditionPresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public string Description => throw new System.NotImplementedException();
    
    
    }

}

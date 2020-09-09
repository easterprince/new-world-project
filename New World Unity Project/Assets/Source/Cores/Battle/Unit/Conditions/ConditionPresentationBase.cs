using NewWorld.Utilities;

namespace NewWorld.Cores.Battle.Unit.Conditions {

    public class ConditionPresentationBase<TPresented> : UnitModulePresentationBase<TPresented>, IConditionPresentation
        where TPresented : class, IConditionPresentation {
        
        // Constructor.
        
        public ConditionPresentationBase(TPresented presented) : base(presented) {}


        // Properties.

        public bool Cancellable => Presented.Cancellable;
        public bool Finished => Presented.Finished;
        public string Description => Presented.Description;
        public float ConditionSpeed => Presented.ConditionSpeed;
        public NamedId Id => Presented.Id;


    }

}

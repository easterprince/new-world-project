using NewWorld;

namespace NewWorld.Battlefield.Units.Conditions {
    
    public class ConditionPresentation<TCondition, TPresentation> : UnitModulePresentation<TCondition, TPresentation>, IConditionPresentation
        where TCondition : Condition<TPresentation>
        where TPresentation : ConditionPresentation<TCondition, TPresentation> {

        // Properties.

        public bool Exited => Presented.Exited;
        public bool CanBeCancelled => Presented.CanBeCancelled;
        public virtual string Description => "Unknown";


        // Constructor.

        public ConditionPresentation(TCondition presented) : base(presented) {}


    }

}

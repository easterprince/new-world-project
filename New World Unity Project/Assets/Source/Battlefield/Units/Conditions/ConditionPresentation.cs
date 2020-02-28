using NewWorld;

namespace NewWorld.Battlefield.Units.Conditions {
    
    public class ConditionPresentation<TCondition, TSelf> : UnitModulePresentation<TCondition, TSelf>, IConditionPresentation
        where TCondition : Condition<TSelf>
        where TSelf : ConditionPresentation<TCondition, TSelf> {

        // Properties.

        public bool Exited => Presented.Exited;
        public bool CanBeCancelled => Presented.CanBeCancelled;
        public virtual string Description => "Unknown";


        // Constructor.

        public ConditionPresentation(TCondition presented) : base(presented) {}


    }

}

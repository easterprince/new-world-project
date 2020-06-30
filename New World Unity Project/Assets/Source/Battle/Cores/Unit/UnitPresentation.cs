using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Durability;

namespace NewWorld.Battle.Cores.Unit {

    public class UnitPresentation : UnitPresentationBase<UnitCore>,
        IReceptive<DamageCausingAction>, IReceptive<ConditionCausingAction> {

        // Constructor.

        public UnitPresentation(UnitCore presented) : base(presented) {}


        // Action processing.

        public void PlanAction(DamageCausingAction action) => Presented.PlanAction(action);
        public void PlanAction(ConditionCausingAction action) => Presented.PlanAction(action);


    }

}

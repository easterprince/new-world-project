using NewWorld.Battle.Cores.UnitSystem;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Others {

    public class IdleCondition :
        ConditionModuleBase<IdleCondition, ConditionPresentationBase<IdleCondition>> {

        // Properties.

        public override bool Cancellable => true;
        public override ConditionId Id => ConditionId.Default;
        public override float ConditionSpeed => 1;
        public override string Description => "Idle.";


        // Cloning.

        public override IdleCondition Clone() {
            return new IdleCondition();
        }


        // Presentation generation.

        private protected override ConditionPresentationBase<IdleCondition> BuildPresentation() {
            return new ConditionPresentationBase<IdleCondition>(this);
        }


        // Updating.

        private protected override void OnAct(out bool finished) {
            ValidateContext();
            finished = false;

            // Fix current node.
            Vector2Int setNodePosition = Context.UnitSystem[Owner];
            Vector2Int realNodePosition = Context.Map.GetNearestPosition(Owner.Body.Position);
            if (setNodePosition != realNodePosition) {
                Context.UnitSystem.PlanAction(new UnitMotionAction(Owner, realNodePosition));
            }

        }
    
    
    }

}

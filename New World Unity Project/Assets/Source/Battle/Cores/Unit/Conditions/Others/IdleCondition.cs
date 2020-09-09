using NewWorld.Battle.Cores.UnitSystem;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Others {

    public class IdleCondition :
        ConditionModuleBase<IdleCondition, ConditionPresentationBase<IdleCondition>> {

        // Fields.

        private ConditionId id;


        // Constructors.

        public IdleCondition(ConditionId id) {
            this.id = id;
        }

        public IdleCondition(IdleCondition other) {
            id = other.id;
        }


        // Properties.

        public override bool Cancellable => true;
        public override float ConditionSpeed => 1;
        public override string Description => "Idle.";
        public override ConditionId Id => id;


        // Cloning.

        public override IdleCondition Clone() {
            return new IdleCondition(this);
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

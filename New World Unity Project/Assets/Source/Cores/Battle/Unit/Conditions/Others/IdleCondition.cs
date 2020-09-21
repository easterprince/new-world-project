using NewWorld.Cores.Battle.UnitSystem;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Conditions.Others {

    public class IdleCondition :
        ConditionModuleBase<IdleCondition, ConditionPresentationBase<IdleCondition>> {

        // Fields.

        private NamedId id;


        // Constructors.

        public IdleCondition(NamedId id) {
            this.id = id;
        }

        public IdleCondition(IdleCondition other) {
            id = other.id;
        }


        // Properties.

        public override bool Cancellable => true;
        public override float ConditionSpeed => 1;
        public override NamedId Id => id;


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

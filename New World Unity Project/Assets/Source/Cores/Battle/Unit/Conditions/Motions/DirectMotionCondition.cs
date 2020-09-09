using NewWorld.Cores.Battle.Map;
using NewWorld.Cores.Battle.Unit.Body;
using NewWorld.Cores.Battle.UnitSystem;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Conditions.Motions {

    public class DirectMotionCondition :
        ConditionModuleBase<DirectMotionCondition, MotionConditionPresentation>, IMotionConditionPresentation {

        // Static.

        public static float ToleratedDestinationOffset => 0.001f;
        public static float ToleratedNodeOffset => 0.6f;


        // Fields.

        private readonly Vector3 destination;
        private readonly float speed;
        private readonly NamedId id;


        // Constructor.

        public DirectMotionCondition(Vector3 destination, float speed, NamedId id) {
            this.destination = destination;
            this.speed = Mathf.Max(speed, 0);
            this.id = id;
        }

        public DirectMotionCondition(DirectMotionCondition other) {
            destination = other.destination;
            speed = other.speed;
            id = other.id;
        }


        // Properties.

        public Vector3 Destination => destination;
        public float MovementPerSecond => speed;
        public override string Description => $"Moving to position {destination}.";
        public override float ConditionSpeed => speed;
        public override NamedId Id => id;
        public override bool Cancellable => true;


        // Updating.

        private protected override void OnAct(out bool finished) {
            ValidateContext();
            Vector3 curPosition = Owner.Body.Position;
            Vector3 toMove = destination - curPosition;
            finished = (toMove.magnitude <= ToleratedDestinationOffset);
            float willMove = speed * Context.GameTimeDelta;
            if (toMove.magnitude > willMove) {
                toMove = toMove.normalized * willMove;
            }
            Vector3 newPosition = curPosition + toMove;
            Vector2Int curNodePosition = Context.UnitSystem[Owner];
            Vector2Int newNodePosition = Context.Map.GetNearestPosition(newPosition);
            if (curNodePosition != newNodePosition) {
                if (Context.Map[newNodePosition].Type == MapNode.NodeType.Common) {
                    Context.UnitSystem.PlanAction(new UnitMotionAction(Owner, newNodePosition));
                }
                float curNodeDistance = (new Vector2(newPosition.x, newPosition.z) - curNodePosition).magnitude;
                if (curNodeDistance > ToleratedNodeOffset) {
                    return;
                }
            }
            Owner.PlanAction(new MovementAction(toMove, adjustRotation: true, adjustVelocity: true));
        }


        // Cloning.

        public override DirectMotionCondition Clone() {
            return new DirectMotionCondition(this);
        }


        // Presentation generation.

        private protected override MotionConditionPresentation BuildPresentation() {
            return new MotionConditionPresentation(this);
        }


    }

}

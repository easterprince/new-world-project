using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.UnitSystem;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Motions {
    
    public class DirectMotionCondition : MotionCondition {

        // Static.

        public float ToleratedDestinationOffset => 0.001f;
        public float ToleratedNodeOffset => 0.6f;


        // Fields.

        private readonly Vector3 destination;
        private readonly float speed;


        // Constructor.

        public DirectMotionCondition(Vector3 destination, float speed) {
            this.destination = destination;
            this.speed = Mathf.Max(speed, 0);
        }

        public DirectMotionCondition(DirectMotionCondition other) {
            destination = other.destination;
            speed = other.speed;
        }


        // Properties.

        public override Vector3 Destination => destination;
        public override float MovementPerSecond => speed;
        public override string Description => $"Moving to position {destination}.";

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

        public override MotionCondition Clone() {
            return new DirectMotionCondition(this);
        }


    }

}

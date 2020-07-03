using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.UnitSystem;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Conditions.Motions {
    
    public class DirectMotionCondition : MotionCondition {

        // Static.

        public float ToleratedNodeOffset => 0.6f;


        // Fields.

        private readonly Vector3 destination;
        private readonly float speed;

        private bool finished;


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
        public override bool Finished => finished;


        // Updating.

        public override void Update() {
            ValidateContext();
            Vector3 curPosition = Owner.Body.Position;
            Vector3 toMove = destination - curPosition;
            if (toMove.magnitude <= ToleratedNodeOffset) {
                finished = true;
            }
            float willMove = speed * Context.GameTimeDelta;
            if (toMove.magnitude > willMove) {
                toMove = toMove.normalized * willMove;
            }
            Vector3 newPosition = curPosition + toMove;
            Vector2Int curNodePosition = Context.UnitSystem[Owner];
            Vector2Int newNodePosition = Context.Map.GetNearestPosition(newPosition);
            if (curNodePosition != newNodePosition) {
                Context.UnitSystem.PlanAction(new UnitMotionAction(Owner, newNodePosition));
                float curNodeDistance = (new Vector2(curPosition.x, curPosition.z) - newNodePosition).magnitude;
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

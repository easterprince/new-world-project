using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions;

namespace NewWorld.Battlefield.Units.Abilities {

    public class SimpleMotion : MotionAbility {

        // Static.

        private static readonly int speedAnimatorHash = Animator.StringToHash("Speed"); 


        // Fields.

        // Parameters.
        private const float speed = 1;

        // Updating.
        private bool nodeUpdated;
        private float lastTime;


        // Constructor.

        public SimpleMotion(UnitController owner) : base(owner) {}


        // Inner methods.

        protected override void OnStart() {
            Owner.Animator?.SetFloat(speedAnimatorHash, speed);
            nodeUpdated = false;
        }

        protected override bool CalculatePoisitonAndRotation(out Vector3 newPosition, out Quaternion newRotation) {
            bool finished = false;

            Vector3 lastPosition = Owner.Position;
            Quaternion lastRotation = Owner.Rotation;
            newPosition = lastPosition;
            newRotation = lastRotation;
            if (!nodeUpdated) {
                return finished;
            }

            // Update time.
            float currentTime = Time.time;
            float deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            // Calculate x and y components.
            Vector2 lastPosition2D = new Vector2(lastPosition.x, lastPosition.z);
            Vector2 newPosition2D;
            float deltaDistance = speed * deltaTime;
            Vector2 path = TargetedNode - lastPosition2D;
            if (path.magnitude <= deltaDistance) {
                newPosition2D = TargetedNode;
                finished = true;
                Owner.Animator?.SetFloat(speedAnimatorHash, 0); // TODO: Move animation finish to OnStop().
            } else {
                newPosition2D = lastPosition2D + deltaDistance * path.normalized;
                newRotation = Quaternion.LookRotation(new Vector3(path.x, 0, path.y));
            }

            // Calculate y component.
            float y = MapController.Instance.GetSurfaceHeight(newPosition2D);

            newPosition = new Vector3(newPosition2D.x, y, newPosition2D.y);
            return finished;
        }


        // Actions management.

        public override IEnumerable<GameAction> ReceiveActions() {
            if (Moves && !nodeUpdated) {
                ConnectedNodeUpdate relocationAction = new ConnectedNodeUpdate(Owner, TargetedNode);
                nodeUpdated = true;
                lastTime = Time.time;
                return Enumerables.GetSingle<GameAction>(relocationAction);
            }
            return Enumerables.GetNothing<GameAction>();
        }


    }

}
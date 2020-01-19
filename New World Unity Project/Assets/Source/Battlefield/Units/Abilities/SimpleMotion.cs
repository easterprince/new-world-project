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


        protected override IEnumerable<GameAction> BuildActions(out bool finished) {
            finished = false;
            List<GameAction> actions = null;

            if (!nodeUpdated) {
                actions = new List<GameAction>();
                ConnectedNodeUpdate connectedNodeUpdate = new ConnectedNodeUpdate(Owner, TargetedNode);
                lastTime = Time.time;
                actions.Add(connectedNodeUpdate);
                nodeUpdated = true;
            }

            // Update time.
            float currentTime = Time.time;
            float deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            Vector3 lastPosition = Owner.Position;
            Quaternion lastRotation = Owner.Rotation;

            // Calculate x and z components.
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
            }

            // Calculate y component.
            float y = MapController.Instance.GetSurfaceHeight(newPosition2D);

            // Calculate position.
            Vector3 newPosition = new Vector3(newPosition2D.x, y, newPosition2D.y);

            // Calculate rotation.
            Quaternion? newRotation = null;
            if (path != Vector2.zero) {
                newRotation = Quaternion.LookRotation(new Vector3(path.x, 0, path.y));
            }

            // Add transform update.
            TransformUpdate transformUpdate = new TransformUpdate(Owner, newPosition, newRotation);
            if (actions == null) {
                return Enumerables.GetSingle<GameAction>(transformUpdate);
            } else {
                actions.Add(transformUpdate);
                return actions;
            }
        }


    }

}
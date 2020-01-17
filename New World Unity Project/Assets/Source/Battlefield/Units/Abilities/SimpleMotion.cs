using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions;

namespace NewWorld.Battlefield.Units.Abilities {

    public class SimpleMotion : MotionAbility {

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
            nodeUpdated = false;
        }

        protected override Vector3 CalculatePoisiton(out bool finished) {
            finished = false;

            Vector3 lastPosition = Owner.Position;
            if (!nodeUpdated) {
                return lastPosition;
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
            } else {
                newPosition2D = lastPosition2D + deltaDistance * path.normalized;
            }

            // Calculate y component.
            float y = MapController.Instance.GetSurfaceHeight(newPosition2D);

            Vector3 newPosition = new Vector3(newPosition2D.x, y, newPosition2D.y);
            return newPosition;
        }


        // Actions management.

        public override IEnumerable<UnitAction> ReceiveActions() {
            if (Moves && !nodeUpdated) {
                RelocationAction relocationAction = new RelocationAction(TargetedNode);
                nodeUpdated = true;
                lastTime = Time.time;
                return Enumerables.GetSingle<UnitAction>(relocationAction);
            }
            return Enumerables.GetNothing<UnitAction>();
        }


    }

}
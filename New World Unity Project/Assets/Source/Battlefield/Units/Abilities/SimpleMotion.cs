using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public class SimpleMotion : MotionAbility {

        // Fields.

        // Node update.
        private ChangingConnectedNodeIntention changingConnectedNodeIntention;

        // Position update.
        private Vector3 lastPosition;
        private float lastTime;


        // Inner methods.

        protected override void OnStart() {
            changingConnectedNodeIntention = new ChangingConnectedNodeIntention(TargetedNode);
            lastPosition = CalculatePoisiton(out _);
        }

        protected override void OnStop() {
            changingConnectedNodeIntention = null;
        }

        protected override Vector3 CalculatePoisiton(out bool targetReached) {
            targetReached = false;
            Vector2 newPosition2D;
            if (!StartedMotion || StartedMotion && changingConnectedNodeIntention != null) {
                newPosition2D = CurrentNode;
            } else {
                Vector2 lastPosition2D = new Vector2(lastPosition.x, lastPosition.y);
                float currentTime = Time.time;
                float deltaTime = currentTime - lastTime;
                lastTime = currentTime;
                float deltaDistance = Speed * deltaTime;
                Vector2 path = TargetedNode - lastPosition2D;
                if (path.sqrMagnitude <= deltaDistance) {
                    newPosition2D = lastPosition2D;
                    targetReached = true;
                } else {
                    newPosition2D = lastPosition2D + deltaDistance * path.normalized;
                }
            }
            float z = Mathf.Max(MapController.Instance.GetSurfaceHeight(newPosition2D), 0);
            lastPosition = new Vector3(newPosition2D.x, newPosition2D.y, z);
            return lastPosition;
        }


        // Intentions management.

        public override Intention ReceiveIntention() {
            return changingConnectedNodeIntention;
        }

        public override void AnswerIntention(Intention intention, bool satisfied) {
            if (intention == changingConnectedNodeIntention) {
                if (satisfied) {
                    changingConnectedNodeIntention = null;
                    lastTime = Time.time;
                }
            }
        }



    }

}
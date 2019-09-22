using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Intentions;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Abilities {

    public class SimpleMotion : MotionAbility {

        // Constants.

        private float LatencyTime = 0.5f;


        // Fields.

        // Node update.
        private UpdateConnectedNodeIntention updateConnectedNodeIntention;

        // Position update.
        private Vector3 lastPosition;
        private float lastTime;


        // Inner methods.

        protected override void OnStart() {
            lastTime = Time.time;
            updateConnectedNodeIntention = new UpdateConnectedNodeIntention(TargetedNode);
            lastPosition = CalculatePoisiton(out _);
        }

        protected override void OnStop() {
            updateConnectedNodeIntention = null;
        }

        protected override Vector3 CalculatePoisiton(out MotionCondition motionCondition) {
            UpdateIntentionState();
            motionCondition = MotionCondition.Moving;
            Vector2 newPosition2D;
            if (updateConnectedNodeIntention != null) {
                newPosition2D = CurrentNode;
                if (Time.time - lastTime > LatencyTime) {
                    motionCondition = MotionCondition.Failed;
                }
            } else {
                Vector2 lastPosition2D = new Vector2(lastPosition.x, lastPosition.y);
                float currentTime = Time.time;
                float deltaTime = currentTime - lastTime;
                lastTime = currentTime;
                float deltaDistance = Speed * deltaTime;
                Vector2 path = TargetedNode - lastPosition2D;
                if (path.magnitude <= deltaDistance) {
                    newPosition2D = lastPosition2D;
                    motionCondition = MotionCondition.TargetReached;
                } else {
                    newPosition2D = lastPosition2D + deltaDistance * path.normalized;
                }
            }
            float z = Mathf.Max(MapController.Instance.GetSurfaceHeight(newPosition2D), 0);
            lastPosition = new Vector3(newPosition2D.x, newPosition2D.y, z);
            return lastPosition;
        }


        // Intentions management.

        public override IEnumerable<Intention> ReceiveIntentions() {
            UpdateIntentionState();
            if (updateConnectedNodeIntention == null) {
                return null;
            }
            return new SingleElementEnumerable<UpdateConnectedNodeIntention>(updateConnectedNodeIntention);
        }

        public override void Fulfil(Intention intention) {
            if (intention == null) {
                throw new System.ArgumentNullException(nameof(intention));
            }
        }


        // Support.

        private void UpdateIntentionState() {
            if (updateConnectedNodeIntention == null) {
                return;
            }
            if (updateConnectedNodeIntention.Satisfied) {
                updateConnectedNodeIntention = null;
                lastTime = Time.time;
            }
        }


    }

}
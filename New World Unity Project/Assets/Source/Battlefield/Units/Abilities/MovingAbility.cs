using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public class MovingAbility : Ability {

        // Fields.

        // Parameters.
        private readonly float speed;

        // Common.
        private Vector2Int targetedNode = Vector2Int.zero;
        private Vector2Int previousTargetedNode = Vector2Int.zero;
        private Vector3 lastPosition = Vector3.zero;

        // Motion-related.
        private bool startedMotion = false;
        private ChangingConnectedNodeIntention changingConnectedNodeIntention = null;
        private bool changedNode = false;
        private float lastTime = 0;


        // Properties.

        public float Speed => speed;
        public Vector3 LastPosition => lastPosition;
        public Vector2Int? TargetedNode => targetedNode;
        public bool Moving => startedMotion;

        // Constructor.

        public MovingAbility(float speed) {
            this.speed = speed;
        }


        // Activating.

        public void Enable(Vector2Int startNode) {
            targetedNode = startNode;
            previousTargetedNode = startNode;
            UpdatePosition();
            Enabled = true;
        }

        protected override void OnDisable() {
            base.OnDisable();
            CancelMotion();
        }


        // Interactions.

        public void StartMotion(Vector2Int startNode) {
            if (startedMotion) {
                CancelMotion();
            }
            targetedNode = startNode;
            startedMotion = true;
            lastTime = Time.time;
        }

        public void CancelMotion() {
            StopMotion();
            UpdatePosition();
        }

        public void UpdatePosition() {
            Vector2 lastPosition2D = new Vector2(lastPosition.x, lastPosition.y);
            Vector2 newPosition2D;
            if (startedMotion || !changedNode) {
                newPosition2D = targetedNode;
            } else {
                float currentTime = Time.time;
                float deltaTime = currentTime - lastTime;
                float deltaDistance = speed * deltaTime;
                Vector2 path = targetedNode - lastPosition2D;
                if (path.sqrMagnitude <= deltaDistance) {
                    newPosition2D = lastPosition2D;
                    StopMotion();
                } else {
                    newPosition2D = lastPosition2D + deltaDistance * path.normalized;
                }
            }
            float z = Mathf.Max(MapController.Instance.GetSurfaceHeight(newPosition2D), 0);
            lastPosition = new Vector3(newPosition2D.x, newPosition2D.y, z);
        }


        // Intentions processing.

        public override Intention ReceiveIntention() {
            if (startedMotion && !changedNode) {
                if (changingConnectedNodeIntention == null) {
                    changingConnectedNodeIntention = new ChangingConnectedNodeIntention(targetedNode);
                }
                return changingConnectedNodeIntention;
            }
            return null;
        }

        public override void SatisfyIntention(Intention intention) {
            if (intention == null || intention != changingConnectedNodeIntention) {
                return;
            }
            if (intention.Satisfied) {
                changedNode = true;
            } else {
                CancelMotion();
            }
        }


        // Suuport.

        private void StopMotion() {
            targetedNode = previousTargetedNode;
            startedMotion = false;
            changedNode = false;
            changingConnectedNodeIntention = null;
            lastTime = 0;
        }


    }

}
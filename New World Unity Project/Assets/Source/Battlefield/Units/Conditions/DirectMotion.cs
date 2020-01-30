using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal;

namespace NewWorld.Battlefield.Units.Conditions {

    public class DirectMotion : Condition {

        // Static.

        private static readonly int motionSpeedAnimatorHash = Animator.StringToHash("MotionSpeed");
        private const float nodeDistanceLimit = 0.6f;


        // Fields.

        // Parameters.
        private readonly Vector2 destination;
        private readonly float speed;

        // Updating.
        private float lastTime;

        // Support.
        private Vector2Int destinationNode;


        // Properties.

        sealed override public bool CanBeCancelled => true;


        // Constructor.

        public DirectMotion(UnitController owner, Vector2 destination, float speed = 1) : base(owner) {
            this.destination = destination;
            this.speed = Mathf.Max(0, speed);
        }


        // Inner methods.

        protected override IEnumerable<GameAction> OnEnter() {
            lastTime = Time.time;
            destinationNode = Vector2Int.RoundToInt(destination);
            var animationParameterUpdate = new AnimatorParameterUpdate<float>(Owner, motionSpeedAnimatorHash, speed);
            return Enumerables.GetSingle(animationParameterUpdate);
        }


        override protected IEnumerable<GameAction> OnUpdate(out bool completed) {
            var actions = Enumerables.GetNothing<GameAction>();

            bool positionReached = false;
            bool nodeReached = false;

            // Calculate time.
            float currentTime = Time.time;
            float deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            // Calculate new x and z components.
            Vector3 lastPosition = Owner.Position;
            Vector2 lastPosition2D = new Vector2(lastPosition.x, lastPosition.z);
            float deltaDistance = speed * deltaTime;
            Vector2 positionChange = destination - lastPosition2D;
            if (positionChange.magnitude <= deltaDistance) {
                positionReached = true;
            } else {
                positionChange *= deltaDistance / positionChange.magnitude;
            }
            Vector2 newPosition2D = lastPosition2D + positionChange;

            // Add unit update.
            Vector2Int connectedNode = UnitSystemController.Instance.GetConnectedNode(Owner);
            float nodeDistance = (connectedNode - newPosition2D).magnitude;
            if (nodeDistance < nodeDistanceLimit) {
                var unitMoving = new UnitMoving(Owner, positionChange, Quaternion.identity);
                actions = Enumerables.Unite(actions, unitMoving);
            }

            // Add connected node update.
            if (connectedNode == destinationNode) {
                nodeReached = true;
            } else {
                var currentNode = Vector2Int.RoundToInt(newPosition2D);
                if (currentNode != connectedNode) {
                    var connectedNodeUpdate = new ConnectedNodeUpdate(Owner, currentNode);
                    actions = Enumerables.Unite(actions, connectedNodeUpdate);
                }
            }

            completed = positionReached && nodeReached;

            return actions;
        }

        override protected IEnumerable<GameAction> OnFinish(StopType stopType) {
            var animationParameterUpdate = new AnimatorParameterUpdate<float>(Owner, motionSpeedAnimatorHash, 0);
            return Enumerables.GetSingle<GameAction>(animationParameterUpdate);
        }


    }

}

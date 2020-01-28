using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;

namespace NewWorld.Battlefield.Units.Abilities.Active.Motions {

    public class SimpleMotion : MotionAbility {

        // Static.

        private static readonly int motionSpeedAnimatorHash = Animator.StringToHash("MotionSpeed");
        private static readonly float tolerantNodeDistanceLimit = 0.55f;


        // Fields.

        // Parameters.
        private const float speed = 1;

        // Updating.
        private float lastTime;


        // Properties.

        sealed override public bool CanBeCancelled => true;


        // Constructor.

        public SimpleMotion(UnitController owner) : base(owner) { }


        // Inner methods.

        override protected IEnumerable<GameAction> OnMotionStart() {
            lastTime = Time.time;

            var animationParameterUpdate = new AnimatorParameterUpdate<float>(Owner, motionSpeedAnimatorHash, speed);
            return Enumerables.GetSingle(animationParameterUpdate);
        }


        override protected IEnumerable<GameAction> OnUpdate(out bool completed) {
            completed = false;
            var actions = Enumerables.GetNothing<GameAction>();

            bool positionReached = false;
            bool nodeReached = false;

            // Calculate time.
            float currentTime = Time.time;
            float deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            Vector3 lastPosition = Owner.Position;
            Quaternion lastRotation = Owner.Rotation;

            // Calculate x and z components.
            Vector2 lastPosition2D = new Vector2(lastPosition.x, lastPosition.z);
            Vector2 newPosition2D;
            float deltaDistance = speed * deltaTime;
            Vector2 path = Destination - lastPosition2D;
            if (path.magnitude <= deltaDistance) {
                newPosition2D = Destination;
                positionReached = true;
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
            var transformUpdate = new TransformUpdate(Owner, newPosition, newRotation);
            actions = Enumerables.Unite(actions, transformUpdate);

            // Add connected node update.
            var connectedNode = UnitSystemController.Instance.GetConnectedNode(Owner);
            if (MaximumMetric.GetNorm(newPosition2D - connectedNode) > tolerantNodeDistanceLimit) {
                var currentNode = Vector2Int.RoundToInt(newPosition2D);
                var connectedNodeUpdate = new ConnectedNodeUpdate(Owner, currentNode);
                actions = Enumerables.Unite(actions, connectedNodeUpdate);
            } else if (connectedNode == Destination) {
                nodeReached = true;
            }

            completed = positionReached && nodeReached;

            return actions;
        }

        override protected IEnumerable<GameAction> OnFinish(StopType stopType) {

            var animationParameterUpdate = new AnimatorParameterUpdate<float>(Owner, motionSpeedAnimatorHash, 0);
            var actions = Enumerables.GetSingle<GameAction>(animationParameterUpdate);

            return actions;
        }


    }

}
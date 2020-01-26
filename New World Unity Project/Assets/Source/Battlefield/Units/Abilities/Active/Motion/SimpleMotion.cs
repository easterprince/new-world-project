using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;

namespace NewWorld.Battlefield.Units.Abilities.Active.Motion {

    public class SimpleMotion : MotionAbility {

        // Static.

        private static readonly int motionSpeedAnimatorHash = Animator.StringToHash("MotionSpeed");


        // Fields.

        // Parameters.
        private const float speed = 1;

        // Updating.
        private float lastTime;
        private bool destinationReached;


        // Properties.

        sealed override public bool CanBeCancelled => true;


        // Constructor.

        public SimpleMotion(UnitController owner) : base(owner) { }


        // Inner methods.

        override protected IEnumerable<GameAction> OnMotionStart() {
            lastTime = Time.time;
            destinationReached = false;

            var animationParameterUpdate = new AnimatorParameterUpdate<float>(Owner, motionSpeedAnimatorHash, speed);
            return Enumerables.GetSingle(animationParameterUpdate);
        }


        override protected IEnumerable<GameAction> OnUpdate(out bool completed) {
            completed = false;
            var actions = Enumerables.GetNothing<GameAction>();

            if (!destinationReached) {

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
                    destinationReached = true;
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

                var transformUpdate = new TransformUpdate(Owner, newPosition, newRotation);
                actions = Enumerables.Unite(actions, transformUpdate);

            }
            if (destinationReached) {
                if (UnitSystemController.Instance.GetConnectedNode(Owner) != Destination) {

                    var connectedNodeUpdate = new ConnectedNodeUpdate(Owner, Destination);
                    actions = Enumerables.Unite(actions, connectedNodeUpdate);

                } else {

                    completed = true;

                }
            }

            return actions;
        }

        override protected IEnumerable<GameAction> OnFinish(StopType stopType) {
            var animationParameterUpdate = new AnimatorParameterUpdate<float>(Owner, motionSpeedAnimatorHash, 0);
            var actions = Enumerables.GetSingle<GameAction>(animationParameterUpdate); 

            var connectedNode = UnitSystemController.Instance.GetConnectedNode(Owner);
            var currentNode = Vector2Int.RoundToInt(new Vector2(Owner.Position.x, Owner.Position.z));
            if (connectedNode != currentNode) {
                float y = MapController.Instance.GetSurfaceHeight(connectedNode);
                var transformUpdate = new TransformUpdate(Owner, new Vector3(connectedNode.x, y, connectedNode.y), null);
                actions = Enumerables.Unite(actions, transformUpdate);
            }

            return actions;
        }


    }

}
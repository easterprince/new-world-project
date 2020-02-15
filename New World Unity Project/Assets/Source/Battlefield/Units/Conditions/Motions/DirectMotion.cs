﻿using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal;

namespace NewWorld.Battlefield.Units.Conditions.Motions {

    public class DirectMotion : MotionCondition {

        // Static.

        private static readonly int motionSpeedAnimatorHash = Animator.StringToHash("MotionSpeed");
        private const float nodeDistanceLimit = 0.6f;


        // Fields.

        // Updating.
        private float lastTime;

        // Support.
        private Vector2Int destinationNode;


        // Properties.

        sealed override public bool CanBeCancelled => true;


        // Constructor.

        public DirectMotion(UnitController owner, Vector2 destination, float speed = 1) : base(owner, destination, speed) {}


        // Inner methods.

        override protected IEnumerable<GameAction> OnEnter() {
            lastTime = Time.time;
            destinationNode = Vector2Int.RoundToInt(Destination);
            var animationParameterUpdate = new UpdateAnimatorParameter<float>(Owner, motionSpeedAnimatorHash, Speed);
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
            float deltaDistance = Speed * deltaTime;
            Vector2 positionChange = Destination - lastPosition2D;
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
                var moveUnit = new MoveUnit(Owner, positionChange, Quaternion.identity);
                actions = Enumerables.Unite(actions, moveUnit);
            }

            // Add connected node update.
            if (connectedNode == destinationNode) {
                nodeReached = true;
            } else {
                var currentNode = Vector2Int.RoundToInt(newPosition2D);
                if (currentNode != connectedNode) {
                    var updateConnectedNode = new UpdateConnectedNode(Owner, currentNode);
                    actions = Enumerables.Unite(actions, updateConnectedNode);
                }
            }

            completed = positionReached && nodeReached;

            return actions;
        }

        override protected IEnumerable<GameAction> OnFinish(StopType stopType) {
            var animationParameterUpdate = new UpdateAnimatorParameter<float>(Owner, motionSpeedAnimatorHash, 0);
            return Enumerables.GetSingle<GameAction>(animationParameterUpdate);
        }


    }

}
﻿using UnityEngine;
using NewWorld.Battlefield.Units.Core;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class MotionAbility : Ability {

        // Enumerator.

        protected enum MotionCondition {
            Moving,
            TargetReached,
            Failed
        }


        // Fields.

        // Parameters.
        private readonly float speed = 1;

        // Motion characteristics.
        private bool startedMotion;
        private Vector2Int currentNode;
        private Vector2Int targetedNode;


        // Properties.

        public float Speed => speed;
        public bool StartedMotion => startedMotion;
        public Vector2Int CurrentNode => currentNode;
        public Vector2Int TargetedNode => targetedNode;


        // Constructor.

        public MotionAbility(UnitAccount unitAccount) : base(unitAccount) {}


        // Interactions.

        public void StartMotion(Vector2Int currentNode, Vector2Int targetedNode) {
            if (startedMotion) {
                StopMotion();
            }
            startedMotion = true;
            this.currentNode = currentNode;
            this.targetedNode = targetedNode;
            OnStart();
        }

        public void StopMotion() {
            if (!startedMotion) {
                return;
            } 
            startedMotion = false;
            OnStop();
        }

        public Vector3 GetPositionInMotion() {
            if (!StartedMotion) {
                throw new System.InvalidOperationException("Not moving - position is undefined!");
            }
            Vector3 position = CalculatePoisiton(out MotionCondition motionCondition);
            if (motionCondition == MotionCondition.Failed || motionCondition == MotionCondition.TargetReached) {
                StopMotion();
            }
            return position;
        }


        // Inner methods.

        protected abstract void OnStart();

        protected abstract void OnStop();

        protected abstract Vector3 CalculatePoisiton(out MotionCondition motionCondition);


    }

}
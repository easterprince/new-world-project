using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class MotionAbility : Ability {

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

        protected Vector2Int CurrentNode {
            get => currentNode;
        }

        protected Vector2Int TargetedNode {
            get => targetedNode;
        }


        // Interactions.

        public void StartMotion(Vector2Int currentNode, Vector2Int targetedNode) {
            startedMotion = true;
            this.currentNode = currentNode;
            this.targetedNode = targetedNode;
            OnStart();
        }

        public void StopMotion() {
            startedMotion = false;
            OnStop();
        }

        public Vector3 GetPositionInMotion() {
            Vector3 position = CalculatePoisiton(out bool targetReached);
            if (targetReached) {
                StopMotion();
            }
            return position;
        }        


        // Inner methods.

        protected abstract void OnStart();

        protected abstract void OnStop();

        protected abstract Vector3 CalculatePoisiton(out bool targetReached);


    }

}
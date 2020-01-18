using UnityEngine;
using NewWorld.Battlefield.Units.Actions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class MotionAbility : Ability {

        // Fields.

        // Motion characteristics.
        private bool moves = false;
        private Vector2Int startingNode;
        private Vector2Int targetedNode;


        // Properties.

        public bool Moves => moves;
        public Vector2Int StartingNode => startingNode;
        public Vector2Int TargetedNode => targetedNode;


        // Constructor.

        public MotionAbility(UnitController owner) : base(owner) {}


        // Interactions.

        public void StartMotion(Vector2Int from, Vector2Int to) {
            if (moves) {
                throw new System.InvalidOperationException("Motion has been started already.");
            }
            moves = true;
            startingNode = from;
            targetedNode = to;
            OnStart();
        }

        public void UpdateLocation() {
            if (!Moves) {
                return;
            }
            bool finished = CalculatePoisitonAndRotation(out Vector3 newPosition, out Quaternion newRotation);
            Owner.transform.position = newPosition;
            Owner.transform.rotation = newRotation;
            if (finished) {
                moves = false;
            }
        }


        // Inner methods.

        abstract protected void OnStart();

        abstract protected bool CalculatePoisitonAndRotation(out Vector3 newPosition, out Quaternion newRotation);


    }

}
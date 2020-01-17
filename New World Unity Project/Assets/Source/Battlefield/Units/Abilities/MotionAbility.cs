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

        public Vector3 UpdatePosition() {
            if (!Moves) {
                return Owner.Position;
            }
            Vector3 position = CalculatePoisiton(out bool finished);
            if (finished) {
                moves = false;
            }
            return position;
        }


        // Inner methods.

        protected abstract void OnStart();

        protected abstract Vector3 CalculatePoisiton(out bool finished);


    }

}
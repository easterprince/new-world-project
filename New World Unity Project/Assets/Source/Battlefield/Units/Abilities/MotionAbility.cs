using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class MotionAbility : Ability {

        // Fields.

        // Motion characteristics.
        private bool moves = false;
        private Vector2Int startingNode;
        private Vector2Int targetedNode;


        // Properties.

        public override bool IsUsed => moves;
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


        // Actions management.

        public override IEnumerable<GameAction> ReceiveActions() {
            if (!moves) {
                return Enumerables.GetNothing<GameAction>();
            }
            IEnumerable<GameAction> actions = BuildActions(out bool finished);
            if (finished) {
                moves = false;
            }
            return actions;
        }


        // Inner methods.

        protected abstract void OnStart();

        protected abstract IEnumerable<GameAction> BuildActions(out bool finished);


    }

}
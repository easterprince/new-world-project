using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Abilities.Active;
using NewWorld.Battlefield.Units;

namespace NewWorld.Battlefield.Units.Abilities.Active.Motion {

    public abstract class MotionAbility : ActiveAbility {

        // Static.

        public static object FormParameterSet(Vector2Int destination) {
            return destination;
        }


        // Fields.

        private bool moves = false;
        private Vector2Int destination;


        // Properties.

        sealed override public bool IsUsed => moves;
        public Vector2Int Destination => destination;


        // Constructor.

        public MotionAbility(UnitController owner) : base(owner) { }


        // Interactions.

        sealed override public void Use(object parameterSet) {
            if (moves) {
                throw new System.InvalidOperationException("Motion has been started already!");
            }
            if (!(parameterSet is Vector2Int destination)) {
                throw new System.ArgumentException($"Parameter must be of class {this.destination.GetType()}.");
            }
            this.moves = true;
            this.destination = destination;
            OnStart();
        }


        // Actions management.

        sealed override public IEnumerable<GameAction> ReceiveActions() {
            if (!moves) {
                throw new System.InvalidOperationException("Motion has not been started!");
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
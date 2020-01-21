﻿using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Abilities.Active;
using NewWorld.Battlefield.Units;

namespace NewWorld.Battlefield.Units.Abilities.Active.Motion {

    public abstract class MotionAbility : UsableActiveAbility<Vector2Int> {

        // Fields.

        // Motion characteristics.
        private bool moves = false;
        private Vector2Int targetedNode;


        // Properties.

        public override bool IsUsed => moves;
        public Vector2Int TargetedNode => targetedNode;


        // Constructor.

        public MotionAbility(UnitController owner) : base(owner) { }


        // Interactions.

        override public void Use(Vector2Int to) {
            if (moves) {
                throw new System.InvalidOperationException("Motion has been started already!");
            }
            moves = true;
            targetedNode = to;
            OnStart();
        }


        // Actions management.

        override public IEnumerable<GameAction> ReceiveActions() {
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
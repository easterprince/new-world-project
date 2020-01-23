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

        private Vector2Int destination;


        // Properties.

        public Vector2Int Destination => destination;


        // Constructor.

        public MotionAbility(UnitController owner) : base(owner) { }


        // Response methods.

        sealed override protected IEnumerable<GameAction> OnStart(object parameterSet) {
            if (!(parameterSet is Vector2Int destination)) {
                throw new System.ArgumentException($"Parameter must be of class {this.destination.GetType()}.");
            }
            this.destination = destination;
            return OnMotionStart();
        }

        protected abstract IEnumerable<GameAction> OnMotionStart();


    }

}
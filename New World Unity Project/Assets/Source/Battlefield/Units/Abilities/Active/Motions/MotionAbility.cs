using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Abilities.Active.Motions {

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
                throw new System.ArgumentException($"Parameter must be of class {typeof(Vector2Int)}.");
            }
            this.destination = destination;
            return OnMotionStart();
        }

        protected abstract IEnumerable<GameAction> OnMotionStart();


    }

}
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal;

namespace NewWorld.Battlefield.Units.Conditions.Motions {

    public abstract class MotionCondition : Condition {

        // Fields.

        private readonly Vector2 destination;
        private readonly float speed;


        // Properties.

        protected Vector2 Destination => destination;
        protected float Speed => speed;


        // To string conversion.

        override public string ToString() {
            return $"Moving to {destination}";
        }


        // Constructor.

        public MotionCondition(UnitController owner, Vector2 destination, float speed = 1) : base(owner) {
            this.destination = destination;
            this.speed = Mathf.Max(0, speed);
        }


    }

}

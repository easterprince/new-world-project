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

        public Vector2 Destination => destination;
        public float Speed => speed;

        override public string Description => "Moving";


        // Constructor.

        public MotionCondition(Vector2 destination, float speed = 1) : base() {
            this.destination = destination;
            this.speed = Mathf.Max(0, speed);
        }


    }

}

using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Unit.Actions;
using NewWorld.Utilities;
using NewWorld.Battlefield.Unit.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Unit.Actions.UnitUpdates.Internal;

namespace NewWorld.Battlefield.Unit.Conditions.Motions {

    public abstract class MotionCondition : UnitCondition {

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

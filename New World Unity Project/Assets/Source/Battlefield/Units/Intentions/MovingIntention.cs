using UnityEngine;

namespace NewWorld.Battlefield.Units.Intentions {

    public class MovingIntention : Intention {

        // Fields.

        private Vector2Int destination;


        // Properties.

        public Vector2Int Destination => destination;


        // Constructors.

        public MovingIntention(UnitController source, Vector2Int destination) : base(source) {
            this.destination = destination;
        }


    }

}

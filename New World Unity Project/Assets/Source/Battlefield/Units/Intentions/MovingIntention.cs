using UnityEngine;
using NewWorld.Battlefield.Units.Abilities;

namespace NewWorld.Battlefield.Units.Intentions {

    public class MovingIntention : Intention<MovingIntention> {

        // Fields.

        private Vector2Int destination;


        // Properties.

        public Vector2Int Destination => destination;


        // Constructors.

        public MovingIntention(Ability<MovingIntention> source, Vector2Int destination) : base(source) {
            this.destination = destination;
        }


    }

}

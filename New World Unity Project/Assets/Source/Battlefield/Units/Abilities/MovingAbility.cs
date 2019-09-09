using UnityEngine;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public class MovingAbility : Ability<MovingIntention> {

        // Fields.

        // Parameters.
        private readonly float speed;

        // Condition.
        private Vector2Int? destination;
        private float processedTime;


        // Properties.

        public bool Moving => destination != null;


        // Constructor.

        public MovingAbility(float speed) {
            this.speed = speed;
            destination = null;
        }


        // Interaction.

        public MovingIntention GetIntention(Vector2Int destination) {
            return new MovingIntention(this, destination);
        }

        public override void SatisfyIntention(MovingIntention intention) {
            this.destination = intention.Destination;
            processedTime = Time.time;
        }

        public Vector2 UpdatePosition(Vector2 currentPosition) {
            if (destination == null) {
                return currentPosition;
            }
            float currentTime = Time.time;
            float deltaTime = currentTime - processedTime;
            float deltaDistance = speed * deltaTime;
            Vector2 path = (Vector2) destination - currentPosition;
            Vector2 newPosition;
            if (path.sqrMagnitude <= deltaDistance) {
                newPosition = destination.Value;
                destination = null;
            } else {
                newPosition = currentPosition + deltaDistance * path.normalized;
            }
            processedTime = currentTime;
            return newPosition;
        }


    }

}
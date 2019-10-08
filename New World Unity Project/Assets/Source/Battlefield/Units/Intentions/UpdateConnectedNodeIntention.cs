using UnityEngine;
using NewWorld.Battlefield.Units.Abilities;

namespace NewWorld.Battlefield.Units.Intentions {

    public class UpdateConnectedNodeIntention : Intention {

        // Fields.

        private Vector2Int newConnectedNode;


        // Properties.

        public Vector2Int NewConnectedNode => newConnectedNode;


        // Constructors.

        public UpdateConnectedNodeIntention(Vector2Int newConnectedNode) : base() {
            this.newConnectedNode = newConnectedNode;
        }


    }

}

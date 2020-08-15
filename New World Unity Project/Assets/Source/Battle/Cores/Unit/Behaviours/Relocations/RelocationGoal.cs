using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Behaviours.Relocations {
    
    public class RelocationGoal : UnitGoal {

        // Fields.

        private readonly Vector2 destination;


        // Constructor.

        public RelocationGoal(Vector2 destination) {
            this.destination = destination;
        }


        // Properties.

        public Vector2 Destination => destination;

        public override string Name => $"Relocate to {destination}";


    }

}

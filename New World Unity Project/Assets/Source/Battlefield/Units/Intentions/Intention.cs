using NewWorld.Battlefield.Units;
using NewWorld.Battlefield.Units.Abilities;

namespace NewWorld.Battlefield.Units.Intentions {

    public class Intention {

        // Fields.

        private bool satisfied = false;


        // Properties.

        public bool Satisfied => satisfied;


        // Constructor.

        public Intention() {}


        // Methods.

        public void Satisfy() {
            satisfied = true;
        }


    }

}

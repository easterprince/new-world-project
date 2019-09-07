using UnityEngine;
using NewWorld.Battlefield.Units;

namespace NewWorld.Battlefield.Units.Intentions {

    public class Intention {

        // Fields.

        private UnitController source;


        // Properties.

        public UnitController Source => source;


        // Constructor.

        public Intention(UnitController source) {
            this.source = source;
        }

    }

}

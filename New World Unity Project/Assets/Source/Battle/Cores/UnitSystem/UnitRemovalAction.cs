using NewWorld.Battle.Cores.Unit;
using UnityEngine;

namespace NewWorld.Battle.Cores.UnitSystem {

    public class UnitRemovalAction : UnitSystemAction {

        // Fields.

        private readonly UnitCore unit;


        // Properties.

        public UnitCore Unit => unit;


        // Constructor.

        public UnitRemovalAction(UnitCore unit) {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
        }


    }

}

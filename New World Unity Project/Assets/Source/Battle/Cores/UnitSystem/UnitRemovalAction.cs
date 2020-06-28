using NewWorld.Battle.Cores.Unit;
using UnityEngine;

namespace NewWorld.Battle.Cores.UnitSystem {

    public class UnitRemovalAction : UnitSystemAction {

        // Fields.

        private readonly UnitPresentation unit;


        // Properties.

        public UnitPresentation Unit => unit;


        // Constructor.

        public UnitRemovalAction(UnitPresentation unit) {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
        }


    }

}

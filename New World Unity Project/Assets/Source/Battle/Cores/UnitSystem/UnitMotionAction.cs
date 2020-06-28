using NewWorld.Battle.Cores.Unit;
using UnityEngine;

namespace NewWorld.Battle.Cores.UnitSystem {

    public class UnitMotionAction : UnitSystemAction {

        // Fields.

        private readonly UnitPresentation unit;
        private readonly Vector2Int position;


        // Properties.

        public UnitPresentation Unit => unit;
        public Vector2Int Position => position;


        // Constructor.

        public UnitMotionAction(UnitPresentation unit, Vector2Int position) {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
            this.position = position;
        }


    }

}

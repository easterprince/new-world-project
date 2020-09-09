using NewWorld.Cores.Battle.Unit;

namespace NewWorld.Cores.Battle.UnitSystem {

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

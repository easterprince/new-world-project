using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;

namespace NewWorld.Battle.Cores.Battlefield {
    
    public class BattlefieldCore : CoreBase<BattlefieldCore, BattlefieldPresentation> {

        // Fields.

        // Modules.
        private ActionPlanner actionPlanner = null;

        // Subcores.
        private MapCore map = null;
        private UnitSystemCore unitSystem = null;


        // Methods.

        private protected override BattlefieldPresentation BuildPresentation() {
            return new BattlefieldPresentation(this);
        }


    }

}

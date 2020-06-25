using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;

namespace NewWorld.Battle.Cores.Battlefield {
    
    public class BattlefieldCore : ReceptiveCoreBase<BattlefieldCore, BattlefieldPresentation> {

        // Fields.

        // Modules.
        private ActionPlanner actionPlanner = null;

        // Subcores.
        private MapCore map = null;
        private UnitSystemCore unitSystem = null;


        // Constructor.

        public BattlefieldCore(ActionPlanner planner) : base(planner) {}


        // Methods.

        public override void ProcessAction(IGameAction action) {
            throw new System.NotImplementedException(); // TODO.
        }

        private protected override BattlefieldPresentation BuildPresentation() {
            return new BattlefieldPresentation(this);
        }


    }

}

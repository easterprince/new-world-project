using NewWorld.Battle.Cores.Battlefield;

namespace NewWorld.Battle.Cores.Unit {

    public class UnitCore : ReceptiveCoreBase<UnitCore, UnitPresentation, UnitAction> {
        
        // Constructor.

        public UnitCore(ActionPlanner planner) : base(planner) {}


        // Presentation generation.

        private protected override UnitPresentation BuildPresentation() {
            return new UnitPresentation(this);
        }


    }

}

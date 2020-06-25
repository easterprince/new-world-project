using NewWorld.Battlefield.Unit.Controller;

namespace NewWorld.Battlefield.Unit {

    public class UnitAction<TSelf> : GameAction<TSelf>
        where TSelf : UnitAction<TSelf> {

        // Properties.

        public UnitController Unit { get; }


        // Constructor.

        public UnitAction(UnitController unit) {
            Unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
        }


    }

}

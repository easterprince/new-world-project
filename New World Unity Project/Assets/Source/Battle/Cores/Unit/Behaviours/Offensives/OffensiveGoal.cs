using System;

namespace NewWorld.Battle.Cores.Unit.Behaviours.Offensives {

    public class OffensiveGoal : UnitGoal {

        // Fields.

        private readonly UnitPresentation target;


        // Constructor.

        public OffensiveGoal(UnitPresentation target) {
            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }


        // Properties.

        public UnitPresentation Target => target;


        // Public.

        public override string Name => $"Destroy {target.Name}";


    }

}

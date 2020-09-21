using NewWorld.Utilities;
using System;

namespace NewWorld.Cores.Battle.Unit.Behaviours.Offensives {

    public class OffensiveGoal : UnitGoal {

        // Fields.

        private readonly UnitPresentation target;


        // Constructor.

        public OffensiveGoal(UnitPresentation target) {
            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }


        // Properties.

        public UnitPresentation Target => target;

        public override NamedId Id => NamedId.Get("OffensiveGoal");


    }

}

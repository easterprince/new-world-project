using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions.Collapses {
    
    public abstract class CollapseCondition : UnitCondition {

        // Fields.

        private readonly float vanishingPeriod;


        // Properties.

        public float VanishingPeriod => vanishingPeriod;

        override public string Description => "Collapsing";


        // Constructor.

        public CollapseCondition(float vanishingPeriod) : base() {
            this.vanishingPeriod = Mathf.Max(vanishingPeriod, 0);
        }


    }

}

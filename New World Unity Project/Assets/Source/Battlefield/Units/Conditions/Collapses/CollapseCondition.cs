using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions.Collapses {
    
    public abstract class CollapseCondition : Condition {

        // Fields.

        private float vanishingPeriod;


        // Properties.

        protected float VanishingPeriod => vanishingPeriod;


        // Constructor.

        public CollapseCondition(UnitController owner, float vanishingPeriod) : base(owner) {
            this.vanishingPeriod = Mathf.Max(vanishingPeriod, 0);
        }


    }

}

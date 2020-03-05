using UnityEngine;

namespace NewWorld.Battlefield.Units.Conditions.Collapses {
    
    public abstract class CollapseCondition : Condition<CollapseConditionPresentation> {

        // Fields.

        private readonly float vanishingPeriod;


        // Properties.

        public float VanishingPeriod => vanishingPeriod;


        // To string conversion.

        override public string ToString() {
            return "Collapsing";
        }


        // Constructor.

        public CollapseCondition(float vanishingPeriod) : base() {
            this.vanishingPeriod = Mathf.Max(vanishingPeriod, 0);
            Presentation = new CollapseConditionPresentation(this);
        }


    }

}

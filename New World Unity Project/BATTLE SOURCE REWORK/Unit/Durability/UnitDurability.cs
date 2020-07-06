using NewWorld.Battlefield.Unit.Conditions;
using NewWorld.Battlefield.Unit.Conditions.Collapses;
using NewWorld.Battlefield.Unit.Core;
using UnityEngine;

namespace NewWorld.Battlefield.Unit.Durability {

    public class UnitDurability : UnitModuleBase<UnitDurability, UnitCore, UnitDurabilityPresentation> {

        // Static.

        private const float minimumDurabilityLimit = 1f;
        private const float maximumDurabilityLimit = 1e18f;

        public static float MinimumDurabilityLimit => minimumDurabilityLimit;
        public static float MaximumDurabilityLimit => maximumDurabilityLimit;


        // Fields.

        private float durabilityLimit = minimumDurabilityLimit;
        private float durability = minimumDurabilityLimit;


        // Properties.

        public float DurabilityLimit {
            get => durabilityLimit;
            protected set {
                durabilityLimit = Mathf.Clamp(value, minimumDurabilityLimit, maximumDurabilityLimit);
                durability = Mathf.Min(durability, durabilityLimit);
            }
        }

        public float Durability {
            get => Mathf.Max(0, durability);
            protected set => durability = Mathf.Min(value, durabilityLimit);
        }

        public bool Broken => durability <= 0;


        // Constructor.

        public UnitDurability(float durabilityLimit) {
            DurabilityLimit = durabilityLimit;
            Durability = durabilityLimit;
        }


        // Interactions.

        public void TakeDamage(ParentPassport<UnitController> parentPassport, float damageValue) {
            ValidatePassport(parentPassport);
            Durability -= damageValue;
        }

        public void Update(out UnitCondition forceCondition) {
            forceCondition = null;
            if (Broken && !(Owner.CurrentCondition is CollapseCondition)) {
                var condition = new SimpleCollapse(1 + Mathf.Log10(durabilityLimit));
                forceCondition = new ConditionChange(Owner, condition);
            }
        }


    }

}

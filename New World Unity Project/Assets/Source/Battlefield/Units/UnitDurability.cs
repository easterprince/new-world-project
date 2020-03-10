using NewWorld.Battlefield.Units.Conditions.Collapses;
using UnityEngine;

namespace NewWorld.Battlefield.Units {
    
    public class UnitDurability : UnitModule<UnitDurability, UnitController> {

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

        public UnitDurability(ParentPassport<UnitController> parentPassport, float durabilityLimit) {
            DurabilityLimit = durabilityLimit;
            Durability = durabilityLimit;
            Connect(parentPassport);
        }


        // Interactions.

        public void TakeDamage(ParentPassport<UnitController> parentPassport, float damageValue) {
            ValidatePassport(parentPassport);
            Durability -= damageValue;
        }


    }

}

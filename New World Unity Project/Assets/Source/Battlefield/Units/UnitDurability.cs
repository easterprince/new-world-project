using UnityEngine;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;

namespace NewWorld.Battlefield.Units {
    
    public class UnitDurability : UnitModule {

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

        public UnitDurability(UnitController owner, float durabilityLimit) : base(owner) {
            DurabilityLimit = durabilityLimit;
            Durability = durabilityLimit;
        }


        // Interactions.

        public void TakeDamage(DamageCausing damage) {
            Durability -= damage.DamageValue;
        }


    }

}

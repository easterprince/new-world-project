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

        private float durabilityLimit;
        private float durability;


        // Properties.

        public float DurabilityLimit {
            get => durabilityLimit;
            set {
                durabilityLimit = Mathf.Clamp(value, minimumDurabilityLimit, maximumDurabilityLimit);
                durability = Mathf.Max(durability, durabilityLimit);
            }
        }

        public float Durability => Mathf.Max(0, durability);
        public bool Destroyed => durability <= 0;


        // Constructor.

        public UnitDurability(UnitController owner, float durabilityLimit) : base(owner) {
            DurabilityLimit = durabilityLimit;
        }


        // Interactions.

        public void TakeDamage(DamageCausing damage) {
            durability -= damage.DamageValue;
        }


    }

}

using NewWorld.Battle.Cores.Unit.Conditions;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Durability {

    public class DurabilityModule : UnitModuleBase<DurabilityModule, DurabilityPresentation, UnitPresentation> {

        // Fields.

        private float durabilityLimit;
        private float durability;


        // Constructors.

        public DurabilityModule(float durabilityLimit = 1) {
            DurabilityLimit = durabilityLimit;
        }

        public DurabilityModule(float durabilityLimit, float durability) : this(durabilityLimit) {
            Durability = durability;
        }

        public DurabilityModule(DurabilityModule other) {
            durabilityLimit = other.durabilityLimit;
            durability = other.durability;
        }


        // Properties.

        public float DurabilityLimit {
            get => durabilityLimit;
            set {
                durabilityLimit = Mathf.Max(durabilityLimit, 1);
                Durability = durability;
            }
        }

        public float Durability {
            get => durability;
            set => durability = Mathf.Clamp(value, 0, durabilityLimit);
        }

        public bool Fallen => durability < 1f;


        // Cloning.

        public override DurabilityModule Clone() {
            return new DurabilityModule(this);
        }


        // Presentation generation.

        private protected override DurabilityPresentation BuildPresentation() {
            return new DurabilityPresentation(this);
        }


        // Updating.

        public void Update() {
            ValidateContext();
            if (durability < 1f) {
                var action = new ConditionCausingAction(new CollapsingCondition());
                Owner.PlanAction(action);
            }
        }


        // Modifying methods.

        public void CauseDamage(Damage damage) {
            Durability -= damage.DamageValue;
        }


    }

}

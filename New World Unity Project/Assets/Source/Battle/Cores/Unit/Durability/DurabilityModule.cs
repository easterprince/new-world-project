using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Conditions.Others;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Durability {

    public class DurabilityModule : UnitModuleBase<DurabilityModule, DurabilityPresentation, UnitPresentation> {

        // Fields.

        private float durabilityLimit = 1;
        private float durability = 1;


        // Constructors.

        public DurabilityModule(float durabilityLimit = 1) {
            DurabilityLimit = durabilityLimit;
        }

        public DurabilityModule(float durabilityLimit, float durability) : this(durabilityLimit) {
            Durability = durability;
        }

        public DurabilityModule(DurabilityModule other) {
            if (other is null) {
                throw new ArgumentNullException(nameof(other));
            }
            durabilityLimit = other.durabilityLimit;
            durability = other.durability;
        }


        // Properties.

        public float DurabilityLimit {
            get => durabilityLimit;
            set {
                durabilityLimit = Mathf.Max(value, 1);
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

        public void Act() {
            ValidateContext();
            if (durability < 1f) {
                var action = new ConditionCausingAction(new CollapseCondition());
                Owner.PlanAction(action);
            }
        }


        // Modifying methods.

        public void CauseDamage(Damage damage) {
            Durability -= damage.DamageValue;
        }


    }

}

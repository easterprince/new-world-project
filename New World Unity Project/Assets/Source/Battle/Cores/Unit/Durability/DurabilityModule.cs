using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Conditions.Others;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Durability {

    public class DurabilityModule : UnitModuleBase<DurabilityModule, DurabilityPresentation, UnitPresentation> {

        // Constants.

        private const float durabilityThreshold = 1;


        // Fields.

        // Meta.
        private NamedId idleConditionId;
        private NamedId collapseCondtionId;

        // Durability properties.
        private float durabilityLimit = durabilityThreshold;
        private float durability = durabilityThreshold;


        // Constructors.

        public DurabilityModule(float durabilityLimit = durabilityThreshold) {
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
                if (float.IsNaN(value)) {
                    value = durabilityThreshold;
                }
                durabilityLimit = Mathf.Max(value, durabilityThreshold);
                Durability = durability;
            }
        }

        public float Durability {
            get => durability;
            set {
                if (float.IsNaN(value)) {
                    value = durabilityThreshold;
                }
                durability = Mathf.Clamp(value, 0, durabilityLimit);
            }
        }

        public bool Fallen => durability < durabilityThreshold;


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
            if (Owner == null) {
                return;
            }

            // Cause collapse.
            if (Fallen && !(Owner.Condition is CollapseConditionPresentation)) {
                var action = new ConditionChangingAction(CreateCollapseCondition(), forceChange: true);
                Owner.PlanAction(action);
            }

        }

        public IConditionModule CreateUsualCondition() {
            if (!Fallen) {
                return new IdleCondition(idleConditionId);
            } else {
                return CreateCollapseCondition();
            }
        }


        // Modifying methods.

        public void CauseDamage(Damage damage) {
            
            // Modify durability.
            bool fallenBefore = Fallen;
            Durability -= damage.DamageValue;
            bool fallenAfter = Fallen;

            // React to damage.
            if (!fallenBefore && fallenAfter) {
                Act();
            }

        }


        // Internal methods.

        private CollapseCondition CreateCollapseCondition() {
            return new CollapseCondition(id: collapseCondtionId, timeUntilExtinction: 5f);
        }


    }

}

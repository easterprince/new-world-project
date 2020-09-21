using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Cores.Battle.Unit.Conditions.Others;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Durability {

    public class DurabilityModule : UnitModuleBase<DurabilityModule, DurabilityPresentation, UnitPresentation> {

        // Constants.

        private const float durabilityThreshold = 1;


        // Fields.

        // Meta.
        private NamedId idleConditionId;
        private NamedId collapseConditionId;

        // Durability properties.
        private float durabilityLimit = durabilityThreshold;
        private float durability = durabilityThreshold;


        // Constructors.

        public DurabilityModule() {
            idleConditionId = NamedId.Default;
            collapseConditionId = NamedId.Default;
        }

        public DurabilityModule(
            NamedId idleConditionId, NamedId collapseConditionId, float durabilityLimit = durabilityThreshold) {

            this.idleConditionId = idleConditionId;
            this.collapseConditionId = collapseConditionId;
            DurabilityLimit = durabilityLimit;
            Durability = DurabilityLimit;

        }

        public DurabilityModule(
            NamedId idleConditionId, NamedId collapseConditionId, float durabilityLimit, float durability) :
            this(idleConditionId, collapseConditionId, durabilityLimit) {

            Durability = durability;

        }

        public DurabilityModule(DurabilityModule other) {
            if (other is null) {
                throw new ArgumentNullException(nameof(other));
            }
            idleConditionId = other.idleConditionId;
            collapseConditionId = other.collapseConditionId;
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
            return new CollapseCondition(id: collapseConditionId, timeUntilExtinction: 5f);
        }


    }

}

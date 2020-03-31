using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Unit.Actions.UnitUpdates;
using NewWorld.Utilities;
using NewWorld.Battlefield.Unit.Abilities.Attacks;
using NewWorld.Battlefield.Unit.Abilities.Motions;
using NewWorld.Battlefield.Unit.Conditions.Attacks;
using NewWorld.Battlefield.Unit.Conditions.Motions;
using NewWorld.Battlefield.UnitSystem;
using NewWorld.Battlefield.Unit.Core;
using NewWorld.Battlefield.Unit.Abilities;

namespace NewWorld.Battlefield.Unit.Intelligence {

    public class UnitIntelligence : UnitModule<UnitIntelligence, UnitCore, UnitIntelligencePresentation> {

        // Constructor.

        public UnitIntelligence() {}


        // Fields.

        private float? nextMovementTime = null;
        private float? nextStopTime = null;


        // Methods.

        public void Ask(out bool cancelCondition, out AbilityUsage? abilityUsage) {
            cancelCondition = false;
            abilityUsage = null;

            MotionAbility motionAbility = Owner.GetAbility<MotionAbility>();
            AttackAbility attackAbility = Owner.GetAbility<AttackAbility>();

            // Fight around.
            if (attackAbility != null) {
                if (Owner.CurrentCondition == null || Owner.CurrentCondition.CanBeCancelled && !(Owner.CurrentCondition is AttackCondition)) {
                    foreach (Vector2Int nodeDifference in Enumerables.InSegment2(-1, 1)) {
                        var currentNode = UnitSystemController.Instance.GetConnectedNode(Owner);
                        var otherNode = currentNode + nodeDifference;
                        var otherUnit = UnitSystemController.Instance.GetUnitOnPosition(otherNode);
                        if (otherUnit != null && otherUnit != Owner && !(otherUnit.Durability?.Broken ?? true)) {
                            var parameterSet = AttackAbility.FormParameterSet(otherUnit);
                            useAbility = new AbilityUsage(attackAbility, parameterSet);
                        }
                    }
                }
            }

            // Wander around.
            if (motionAbility != null) {
                if (Owner.CurrentCondition == null || Owner.CurrentCondition.CanBeCancelled) {
                    if (nextMovementTime == null) {
                        nextMovementTime = Time.time + Random.Range(0f, 2f);
                    }
                    if (Time.time >= nextMovementTime.Value) {
                        Vector2Int curConnectedNode = UnitSystemController.Instance.GetConnectedNode(Owner);
                        Vector2Int newConnectedNode;
                        do {
                            newConnectedNode = curConnectedNode + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
                        } while (
                            MapController.Instance[newConnectedNode].Type == NodeDescription.NodeType.Abyss ||
                            UnitSystemController.Instance.GetUnitOnPosition(newConnectedNode) != null
                        );
                        object parameterSet = MotionAbility.FormParameterSet(newConnectedNode);
                        useAbility = new AbilityUsage(motionAbility, parameterSet);
                        nextMovementTime = null;
                    }
                }
            }

            // Get stupid around.
            if (nextStopTime == null) {
                nextStopTime = Time.time + Random.Range(0f, 10f);
            }
            if (Time.time >= nextStopTime.Value) {
                if (Owner.CurrentCondition != null) {
                    cancelCondition = new ConditionCancellation(Owner.CurrentCondition);
                }
                nextStopTime = null;
            }

        }


        // Presentation building.

        override private protected UnitIntelligencePresentation BuildPresentation() {
            return new UnitIntelligencePresentation(this);
        }


    }

}

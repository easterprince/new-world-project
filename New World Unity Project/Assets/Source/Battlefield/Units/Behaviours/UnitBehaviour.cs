using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Abilities.Active.Motions;
using NewWorld.Battlefield.Units.Abilities.Active.Attacks;

namespace NewWorld.Battlefield.Units.Behaviours {

    public class UnitBehaviour : UnitModule {

        // Constructor.

        public UnitBehaviour(UnitController owner) : base(owner) {}


        // Behaviour.


        private float? nextMovementTime = null;
        private float? nextStopTime = null;

        public void Act(out AbilityCancellation abilityCancellation, out AbilityUsage abilityUsage) {
            abilityCancellation = null;
            abilityUsage = null;

            // Fight around.
            if (Owner.AttackAbility != null) {
                if (Owner.UsedAbility == null) {
                    foreach (Vector2Int nodeDifference in Enumerables.InSegment2(-1, 1)) {
                        var currentNode = UnitSystemController.Instance.GetConnectedNode(Owner);
                        var otherNode = currentNode + nodeDifference;
                        var otherUnit = UnitSystemController.Instance.GetUnitOnPosition(otherNode);
                        if (otherUnit != null && otherUnit != Owner) {
                            var parameterSet = AttackAbility.FormParameterSet(otherUnit);
                            abilityUsage = new AbilityUsage(Owner.AttackAbility, parameterSet);
                        }
                    }
                }
            }


            // Wander around.
            if (Owner.MotionAbility != null) {
                if (!Owner.MotionAbility.IsUsed && nextMovementTime == null) {
                    nextMovementTime = Time.time + Random.Range(0f, 2f);
                }
                if (Owner.UsedAbility == null && Time.time >= nextMovementTime.Value) {
                    Vector2Int curConnectedNode = UnitSystemController.Instance.GetConnectedNode(Owner);
                    Vector2Int newConnectedNode;
                    do {
                        newConnectedNode = curConnectedNode + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
                    } while (
                        MapController.Instance.GetSurfaceNode(newConnectedNode) == null ||
                        UnitSystemController.Instance.GetUnitOnPosition(newConnectedNode) != null
                    );
                    object parameterSet = MotionAbility.FormParameterSet(newConnectedNode);
                    abilityUsage = new AbilityUsage(Owner.MotionAbility, parameterSet);
                    nextMovementTime = null;
                }
            }

            // Get stupid around.
            if (nextStopTime == null) {
                nextStopTime = Time.time + Random.Range(0f, 10f);
            }
            if (Time.time >= nextStopTime.Value) {
                if (Owner.UsedAbility != null) {
                    abilityCancellation = new AbilityCancellation(Owner.MotionAbility);
                }
                nextStopTime = null;
            }


        }


    }

}

using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities.Active.Motion;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Behaviours {

    public class UnitBehaviour : UnitModule {

        // Constructor.

        public UnitBehaviour(UnitController owner) : base(owner) {}


        // Behaviour.


        private bool plannedMotion = false;
        private float nextMovementTime;
        private float nextStopTime = float.PositiveInfinity;

        public void Act(out AbilityCancellation abilityCancellation, out AbilityUsage abilityUsage) {
            abilityCancellation = null;
            abilityUsage = null;

            // Wander around.
            if (Owner.MotionAbility != null) {
                if (!plannedMotion && !Owner.MotionAbility.IsUsed) {
                    plannedMotion = true;
                    nextMovementTime = Time.time + Random.Range(0f, 2f);
                }
                if (plannedMotion && Time.time >= nextMovementTime && Owner.UsedAbility == null) {
                    plannedMotion = false;
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
                    nextStopTime = Time.time + Random.Range(0f, 2f);
                    // nextStopTime = float.PositiveInfinity;
                }
                if (Time.time >= nextStopTime && Owner.MotionAbility.IsUsed) {
                    abilityCancellation = new AbilityCancellation(Owner.MotionAbility);
                    nextStopTime = float.PositiveInfinity;
                }
            }

        }


    }

}

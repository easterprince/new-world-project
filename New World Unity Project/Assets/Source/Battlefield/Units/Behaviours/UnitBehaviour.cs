﻿using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities.Active.Motion;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units.Behaviours {

    public class UnitBehaviour {

        // Fields.

        private readonly UnitController unit;


        // Constructor.

        public UnitBehaviour(UnitController unit) {
            this.unit = unit ?? throw new System.ArgumentNullException(nameof(unit));
        }


        // Behaviour.


        private bool plannedMotion = false;
        private float nextMovementTime;
        private float nextStopTime = float.PositiveInfinity;

        public void Act(out AbilityCancellation abilityCancellation, out AbilityUsage abilityUsage) {
            abilityCancellation = null;
            abilityUsage = null;

            // Wander around.
            if (unit.MotionAbility != null) {
                if (!plannedMotion && !unit.MotionAbility.IsUsed) {
                    plannedMotion = true;
                    nextMovementTime = Time.time + Random.Range(0f, 2f);
                }
                if (plannedMotion && Time.time >= nextMovementTime && unit.UsedAbility == null) {
                    plannedMotion = false;
                    Vector2Int curConnectedNode = UnitSystemController.Instance.GetConnectedNode(unit);
                    Vector2Int newConnectedNode;
                    do {
                        newConnectedNode = curConnectedNode + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
                    } while (
                        MapController.Instance.GetSurfaceNode(newConnectedNode) == null ||
                        UnitSystemController.Instance.GetUnitOnPosition(newConnectedNode) != null
                    );
                    object parameterSet = MotionAbility.FormParameterSet(newConnectedNode);
                    abilityUsage = new AbilityUsage(unit.MotionAbility, parameterSet);
                    // nextStopTime = Time.time + Random.Range(0f, 2f);
                    nextStopTime = float.PositiveInfinity;
                }
                if (Time.time >= nextStopTime && unit.MotionAbility.IsUsed) {
                    abilityCancellation = new AbilityCancellation(unit.MotionAbility);
                    nextStopTime = float.PositiveInfinity;
                }
            }

        }


    }

}

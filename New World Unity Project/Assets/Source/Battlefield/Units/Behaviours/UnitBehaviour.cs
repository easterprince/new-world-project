using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;

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
        private float nextMovementTime = 0;

        public void Act(out Ability useAbility) {
            useAbility = null;

            // Wander around.
            if (unit.MotionAbility == null || unit.MotionAbility.IsUsed) {
                return;
            }
            if (!plannedMotion) {
                plannedMotion = true;
                nextMovementTime = Time.time + Random.Range(0f, 1.5f);
            }
            if (plannedMotion) {
                if (Time.time >= nextMovementTime) {
                    plannedMotion = false;
                    Vector2Int curConnectedNode = UnitSystemController.Instance.GetConnectedNode(unit);
                    Vector2Int newConnectedNode;
                    do {
                        newConnectedNode = curConnectedNode + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
                    } while (
                        MapController.Instance.GetSurfaceNode(newConnectedNode) == null || 
                        UnitSystemController.Instance.GetUnitOnPosition(newConnectedNode) != null
                    );
                    unit.MotionAbility.StartMotion(curConnectedNode, newConnectedNode);
                    useAbility = unit.MotionAbility;
                }
            }

        }


    }

}

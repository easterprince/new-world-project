using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.General;
using NewWorld.Battlefield.Units.Abilities.Attacks;
using NewWorld.Battlefield.Units.Abilities.Motions;
using NewWorld.Battlefield.Units.Conditions.Attacks;
using NewWorld.Battlefield.Units.Conditions.Motions;

namespace NewWorld.Battlefield.Units.Behaviours {

    public class UnitBehaviour : UnitModule<UnitBehaviourPresentation> {

        // Constructor.

        public UnitBehaviour(UnitController owner) {}


        // Fields.

        private float? nextMovementTime = null;
        private float? nextStopTime = null;


        // Properties.

        public override UnitBehaviourPresentation Presentation => new UnitBehaviourPresentation(this);


        // Methods.

        public void Act(out CancelCondition cancelCondition, out UseAbility useAbility) {
            cancelCondition = null;
            useAbility = null;

            // Fight around.
            if (Owner.AttackAbility != null) {
                if (Owner.CurrentCondition == null || Owner.CurrentCondition.CanBeCancelled && !(Owner.CurrentCondition is AttackCondition)) {
                    foreach (Vector2Int nodeDifference in Enumerables.InSegment2(-1, 1)) {
                        var currentNode = UnitSystemController.Instance.GetConnectedNode(Owner);
                        var otherNode = currentNode + nodeDifference;
                        var otherUnit = UnitSystemController.Instance.GetUnitOnPosition(otherNode);
                        if (otherUnit != null && !otherUnit.Collapsed && otherUnit != Owner) {
                            var parameterSet = AttackAbility.FormParameterSet(otherUnit);
                            useAbility = new UseAbility(Owner.AttackAbility, parameterSet);
                        }
                    }
                }
            }

            // Wander around.
            if (Owner.MotionAbility != null) {
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
                        useAbility = new UseAbility(Owner.MotionAbility, parameterSet);
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
                    cancelCondition = new CancelCondition(Owner.CurrentCondition);
                }
                nextStopTime = null;
            }

        }


    }

}

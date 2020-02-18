using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Map;
using System.Collections.Generic;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController {

        // Actions processing.
        // Note: method have to return true if action has been processed, and false otherwise.

        public void ProcessGameAction(GameAction gameAction) {
            if (gameAction == null) {
                throw new System.ArgumentNullException(nameof(gameAction));
            }
            bool processed = false;

            if (gameAction is UnitSystemUpdate unitSystemUpdate) {
                processed = ProcessUnitSystemUpdate(unitSystemUpdate);
            } else if (gameAction is UnitUpdate unitUpdate) {
                processed = ProcessUnitUpdate(unitUpdate);
            }

            if (!processed) {
                Debug.LogWarning($"Action of type {gameAction.GetType()} was not processed!", this);
            }
        }

        public void ProcessGameActions(IEnumerable<GameAction> gameActions) {
            if (gameActions == null) {
                throw new System.ArgumentNullException(nameof(gameActions));
            }
            foreach (GameAction gameAction in gameActions) {
                ProcessGameAction(gameAction);
            }
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            if (unitSystemUpdate is UpdateConnectedNode connectedNodeUpdate) {
                return ProcessUnitSystemUpdate(connectedNodeUpdate);
            }
            if (unitSystemUpdate is AddUnit unitAddition) {
                return ProcessUnitSystemUpdate(unitAddition);
            }
            if (unitSystemUpdate is RemoveUnit unitRemoval) {
                return ProcessUnitSystemUpdate(unitRemoval);
            }
            return false;
        }

        private bool ProcessUnitSystemUpdate(UpdateConnectedNode connectedNodeUpdate) {
            UnitController updatedUnit = connectedNodeUpdate.Unit;
            Vector2Int newConnectedNode = connectedNodeUpdate.NewConnectedNode;
            if (CheckRelocation(newConnectedNode, updatedUnit)) {
                Vector2Int oldConnectedNode = positions[updatedUnit];
                onPositions.Remove(oldConnectedNode);
                onPositions[newConnectedNode] = updatedUnit;
                positions[updatedUnit] = newConnectedNode;
                connectedNodeUpdatedEvent.TryInvoke(updatedUnit, oldConnectedNode);
            }
            return true;
        }

        private bool ProcessUnitSystemUpdate(AddUnit unitAddition) {
            UnitDescription description = unitAddition.Description;
            if (CheckRelocation(description.ConnectedNode)) {
                UnitController unit = UnitController.BuildUnit(transform, description, $"Unit {unusedUnitIndex++}");
                unit.transform.parent = transform;
                units.Add(unit);
                positions[unit] = description.ConnectedNode;
                onPositions[description.ConnectedNode] = unit;
                unitAddedEvent.TryInvoke(unit);
            }
            return true;
        }

        private bool ProcessUnitSystemUpdate(RemoveUnit unitRemoval) {
            var unit = unitRemoval.Unit;
            if (units.Contains(unit)) {
                Vector2Int position = positions[unit];
                onPositions.Remove(position);
                positions.Remove(unit);
                units.Remove(unit);
                Destroy(unit.gameObject);
                unitRemovedEvent.TryInvoke(unit, position);
            }
            return true;
        }


        // Unit updates.

        private bool ProcessUnitUpdate(UnitUpdate unitUpdate) {
            UnitController unit = unitUpdate.Unit;
            if (units.Contains(unit)) {
                unit.ProcessGameAction(unitUpdate);
            }
            return true;
        }


        // Support methods.

        private bool CheckRelocation(Vector2Int newConnectedNode, UnitController unit = null) {
            if (!MapController.Instance.IsRealNodePosition(newConnectedNode)) {
                return false;
            }
            NodeDescription node = MapController.Instance[newConnectedNode];
            if (node.Type == NodeDescription.NodeType.Abyss) {
                return false;
            }
            if (onPositions.TryGetValue(newConnectedNode, out UnitController otherUnit) && otherUnit != unit) {
                return false;
            }
            return true;
        }


    }

}

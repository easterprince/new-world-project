﻿using UnityEngine;
using NewWorld.Battlefield.Unit.Actions.UnitUpdates;
using NewWorld.Battlefield.Map;
using System.Collections.Generic;
using NewWorld.Battlefield.Unit;

namespace NewWorld.Battlefield.UnitSystem {

    public partial class UnitSystemController {

        // Actions processing.
        // Note: method have to return true if action has been processed, and false otherwise.

        private void ProcessGameAction(GameAction gameAction) {
            if (gameAction == null) {
                throw new System.ArgumentNullException(nameof(gameAction));
            }
            bool processed = false;

            if (gameAction is UnitSystemUpdate unitSystemUpdate) {
                processed = ProcessUnitSystemUpdate(unitSystemUpdate);
            }

            if (!processed) {
                Debug.LogWarning($"Action of type {gameAction.GetType()} was not processed!", this);
            }
        }

        private void ProcessGameActions(IEnumerable<GameAction> gameActions) {
            if (gameActions == null) {
                throw new System.ArgumentNullException(nameof(gameActions));
            }
            foreach (var gameAction in gameActions) {
                ProcessGameAction(gameAction);
            }
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            if (unitSystemUpdate is ConnectedNodeUpdate connectedNodeUpdate) {
                return ProcessUnitSystemUpdate(connectedNodeUpdate);
            }
            if (unitSystemUpdate is UnitAddition unitAddition) {
                return ProcessUnitSystemUpdate(unitAddition);
            }
            if (unitSystemUpdate is UnitRemoval unitRemoval) {
                return ProcessUnitSystemUpdate(unitRemoval);
            }
            return false;
        }

        private bool ProcessUnitSystemUpdate(ConnectedNodeUpdate connectedNodeUpdate) {
            UnitController updatedUnit = connectedNodeUpdate.Unit;
            Vector2Int newConnectedNode = connectedNodeUpdate.NewConnectedNode;
            if (units.Contains(updatedUnit) && CheckRelocation(newConnectedNode, updatedUnit)) {
                Vector2Int oldConnectedNode = positions[updatedUnit];
                onPositions.Remove(oldConnectedNode);
                onPositions[newConnectedNode] = updatedUnit;
                positions[updatedUnit] = newConnectedNode;
                connectedNodeUpdatedEvent.TryInvoke(updatedUnit, oldConnectedNode);
            }
            return true;
        }

        private bool ProcessUnitSystemUpdate(UnitAddition unitAddition) {
            UnitTemplate description = unitAddition.Description;
            if (CheckRelocation(description.ConnectedNode)) {
                UnitController unit = UnitController.BuildUnit(unitsGameObject.transform, description);
                units.Add(unit);
                positions[unit] = description.ConnectedNode;
                onPositions[description.ConnectedNode] = unit;
                unitAddedEvent.TryInvoke(unit);
            }
            return true;
        }

        private bool ProcessUnitSystemUpdate(UnitRemoval unitRemoval) {
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
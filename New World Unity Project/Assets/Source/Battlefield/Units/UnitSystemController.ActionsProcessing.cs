using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Map;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController {

        // Actions processing.
        // Note: method have to return true if action has been processed, and false otherwise.

        public bool ProcessGameAction(GameAction gameAction) {
            if (gameAction == null) {
                throw new System.ArgumentNullException(nameof(gameAction));
            }
            if (gameAction is UnitSystemUpdate unitSystemUpdate) {
                return ProcessUnitSystemUpdate(unitSystemUpdate);
            }
            if (gameAction is UnitUpdate unitUpdate) {
                return ProcessUnitUpdate(unitUpdate);
            }
            return false;
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            if (unitSystemUpdate == null) {
                throw new System.ArgumentNullException(nameof(unitSystemUpdate));
            }
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
            if (connectedNodeUpdate == null) {
                throw new System.ArgumentNullException(nameof(connectedNodeUpdate));
            }
            UnitController updatedUnit = connectedNodeUpdate.Unit;
            Vector2Int newConnectedNode = connectedNodeUpdate.NewConnectedNode;
            if (ValidateRelocation(newConnectedNode, updatedUnit)) {
                onPositions.Remove(positions[updatedUnit]);
                onPositions[newConnectedNode] = updatedUnit;
                positions[updatedUnit] = newConnectedNode;
            }
            return true;
        }

        private bool ProcessUnitSystemUpdate(AddUnit unitAddition) {
            if (unitAddition == null) {
                throw new System.ArgumentNullException(nameof(unitAddition));
            }
            UnitDescription description = unitAddition.Description;
            if (ValidateRelocation(description.ConnectedNode)) {
                UnitController unit = UnitController.BuildUnit(transform, description, $"Unit {unusedUnitIndex++}");
                unit.transform.parent = transform;
                units.Add(unit);
                positions[unit] = description.ConnectedNode;
                onPositions[description.ConnectedNode] = unit;
            }
            return true;
        }

        private bool ProcessUnitSystemUpdate(RemoveUnit unitRemoval) {
            if (unitRemoval == null) {
                throw new System.ArgumentNullException(nameof(unitRemoval));
            }
            var unit = unitRemoval.Unit;
            if (units.Contains(unit)) {
                onPositions.Remove(positions[unit]);
                positions.Remove(unit);
                units.Remove(unit);
                Destroy(unit.gameObject);
            }
            return true;
        }


        // Unit System updates.

        private bool ProcessUnitUpdate(UnitUpdate unitUpdate) {
            if (unitUpdate == null) {
                throw new System.ArgumentNullException(nameof(unitUpdate));
            }
            UnitController unit = unitUpdate.Unit;
            if (units.Contains(unit)) {
                unit.ProcessGameAction(unitUpdate);
            }
            return true;
        }


        // Support methods.

        private bool ValidateRelocation(Vector2Int newConnectedNode, UnitController unit = null) {
            NodeDescription node = MapController.Instance.GetSurfaceNode(newConnectedNode);
            if (node == null) {
                return false;
            }
            if (onPositions.TryGetValue(newConnectedNode, out UnitController otherUnit) && otherUnit != unit) {
                return false;
            }
            return true;
        }


    }

}

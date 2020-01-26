using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController {

        // Actions processing.
        // Note: method have to return true if action has been successfully processed, and false otherwise.

        private bool ProcessGameAction(GameAction gameAction) {
            if (gameAction == null) {
                throw new System.ArgumentNullException(nameof(gameAction));
            }
            if (gameAction is UnitSystemUpdate unitSystemUpdate) {
                return ProcessUnitSystemUpdate(unitSystemUpdate);
            }
            return false;
        }


        // Unit System updates.

        private bool ProcessUnitSystemUpdate(UnitSystemUpdate unitSystemUpdate) {
            if (unitSystemUpdate == null) {
                throw new System.ArgumentNullException(nameof(unitSystemUpdate));
            }
            if (unitSystemUpdate is ConnectedNodeUpdate connectedNodeUpdate) {
                return ProcessUnitSystemUpdate(connectedNodeUpdate);
            }
            return false;
        }

        private bool ProcessUnitSystemUpdate(ConnectedNodeUpdate connectedNodeUpdate) {
            if (connectedNodeUpdate == null) {
                throw new System.ArgumentNullException(nameof(connectedNodeUpdate));
            }
            UnitController updatedUnit = connectedNodeUpdate.UpdatedUnit;
            Vector2Int newConnectedNode = connectedNodeUpdate.NewConnectedNode;
            if (ValidateRelocation(newConnectedNode, updatedUnit)) {
                onPositions.Remove(positions[updatedUnit]);
                onPositions[newConnectedNode] = updatedUnit;
                positions[updatedUnit] = newConnectedNode;
            }
            return true;
        }


    }

}

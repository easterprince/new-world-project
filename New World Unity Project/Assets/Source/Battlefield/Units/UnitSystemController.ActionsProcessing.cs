using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController {

        // Actions processing.
        // Note: method have to return true if action has been successfully processed, and false otherwise.

        private bool ProcessGameAction(GameAction gameAction) {
            if (gameAction is UnitUpdate unitUpdate) {
                return ProcessGameAction(unitUpdate);
            }
            return false;
        }

        private bool ProcessGameAction(UnitUpdate unitUpdate) {
            if (unitUpdate is ConnectedNodeUpdate connectedNodeUpdate) {
                return ProcessGameAction(connectedNodeUpdate);
            }
            return false;
        }

        private bool ProcessGameAction(ConnectedNodeUpdate connectedNodeUpdate) {
            UnitController updatedUnit = connectedNodeUpdate.UpdatedUnit;
            Vector2Int newConnectedNode = connectedNodeUpdate.NewConnectedNode;
            ValidateRelocation(newConnectedNode, updatedUnit);
            onPositions.Remove(positions[updatedUnit]);
            onPositions[newConnectedNode] = updatedUnit;
            positions[updatedUnit] = newConnectedNode;
            return true;
        }


    }

}

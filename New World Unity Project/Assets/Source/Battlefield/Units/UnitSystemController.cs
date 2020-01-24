using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController : SceneSingleton<UnitSystemController> {

        // Fields.

        private HashSet<UnitController> units;
        private Dictionary<UnitController, Vector2Int> positions;
        private Dictionary<Vector2Int, UnitController> onPositions;


        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
            units = new HashSet<UnitController>();
            positions = new Dictionary<UnitController, Vector2Int>();
            onPositions = new Dictionary<Vector2Int, UnitController>();
        }

        private void Update() {
            ProcessActions();
        }


        // Information.

        public Vector2Int GetConnectedNode(UnitController unitController) {
            return positions[unitController];
        }

        public UnitController GetUnitOnPosition(Vector2Int position) {
            if (!onPositions.TryGetValue(position, out UnitController unit)) {
                return null;
            }
            return unit;
        }


        // Actions collecting and applying.

        private void ProcessActions() {
            foreach (UnitController actingUnit in units) {
                IEnumerable<GameAction> actions = actingUnit.ReceiveActions();
                foreach (GameAction action in actions) {
                    if (!ProcessGameAction(action)) {
                        Debug.LogWarning($"Action { action.GetType() } has not been processed. Is it redundant?");
                    }
                }
            }
        }


        // External control.

        public IEnumerator Load(List<UnitDescription> unitDescriptions) {
            if (unitDescriptions == null) {
                throw new System.ArgumentNullException(nameof(unitDescriptions));
            }

            int index = 0;
            foreach (UnitDescription unitDescription in unitDescriptions) {
                if (!ValidateRelocation(unitDescription.ConnectedNode)) {
                    throw new System.InvalidOperationException($"Cannot connect unit to the given node ({ unitDescription.ConnectedNode }).");
                }
                UnitController unit = UnitController.BuildUnit(unitDescription, $"Unit {index++}");
                unit.transform.parent = transform;
                units.Add(unit);
                positions[unit] = unitDescription.ConnectedNode;
                onPositions[unitDescription.ConnectedNode] = unit;
            }

            yield break;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Map;

namespace NewWorld.Battlefield.Units {

    public class UnitSystemController : SceneSingleton<UnitSystemController> {

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
            foreach (UnitController unit in units) {
                IEnumerable<UnitAction> actions = unit.ReceiveActions();
                foreach (UnitAction action in actions) {
                    if (action is RelocationAction relocationAction) {
                        ValidateRelocation(relocationAction.NewConnectedNode, unit);
                        onPositions.Remove(positions[unit]);
                        onPositions[relocationAction.NewConnectedNode] = unit;
                        positions[unit] = relocationAction.NewConnectedNode;
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
                ValidateRelocation(unitDescription.ConnectedNode);
                UnitController unit = UnitController.BuildUnit(unitDescription, $"Unit {index++}");
                unit.transform.parent = transform;
                units.Add(unit);
                positions[unit] = unitDescription.ConnectedNode;
                onPositions[unitDescription.ConnectedNode] = unit;
            }

            yield break;
        }


        // Support methods.

        private void ValidateRelocation(Vector2Int newConnectedNode, UnitController unit = null) {
            NodeDescription node = MapController.Instance.GetSurfaceNode(newConnectedNode);
            if (node == null) {
                throw new System.InvalidOperationException("Connected node does not exist!");
            }
            if (onPositions.TryGetValue(newConnectedNode, out UnitController otherUnit) && otherUnit != unit) {
                throw new System.InvalidOperationException("No two units may be connected to the same node.");
            }
        }


    }

}

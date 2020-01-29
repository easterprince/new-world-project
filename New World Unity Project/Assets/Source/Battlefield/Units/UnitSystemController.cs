using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController : SceneSingleton<UnitSystemController> {

        // Fields.

        private long unusedUnitIndex = 0;
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
            var collectedActions = new List<GameAction>();

            foreach (UnitController actingUnit in units) {
                IEnumerable<GameAction> actions = actingUnit.ReceiveActions();
                collectedActions.AddRange(actions);
            }

            foreach (GameAction action in collectedActions) {
                if (!ProcessGameAction(action)) {
                    Debug.LogWarning($"Action { action.GetType() } has not been processed. Is it redundant?");
                }
            }

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


        // External control.

        public IEnumerator Load(List<UnitDescription> unitDescriptions) {
            if (unitDescriptions == null) {
                throw new System.ArgumentNullException(nameof(unitDescriptions));
            }

            foreach (UnitDescription unitDescription in unitDescriptions) {
                ProcessUnitSystemUpdate(new UnitAddition(unitDescription));
            }

            yield break;
        }


    }

}

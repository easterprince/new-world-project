using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletons;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController : LoadableSingleton<UnitSystemController, List<UnitDescription>> {

        // Fields.

        private long unusedUnitIndex = 0;
        private HashSet<UnitController> units = new HashSet<UnitController>();
        private Dictionary<UnitController, Vector2Int> positions = new Dictionary<UnitController, Vector2Int>();
        private Dictionary<Vector2Int, UnitController> onPositions = new Dictionary<Vector2Int, UnitController>();


        // Life cycle.

        override private protected void Awake() {
            base.Awake();
            Instance = this;
        }

        private void Update() {
            var collectedActions = new List<GameAction>();

            foreach (UnitController actingUnit in units) {
                IEnumerable<GameAction> actions = actingUnit.ReceiveActions();
                collectedActions.AddRange(actions);
            }

            foreach (GameAction action in collectedActions) {
                ProcessGameAction(action);
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

        override public void StartReloading(List<UnitDescription> unitDescriptions) {
            if (unitDescriptions == null) {
                throw new System.ArgumentNullException(nameof(unitDescriptions));
            }

            Loaded = false;

            sweet jesus

            var removeUnits = new List<UnitController>(units);
            foreach (UnitController unit in removeUnits) {
                ProcessGameAction(new RemoveUnit(unit));
            }
            foreach (UnitDescription unitDescription in unitDescriptions) {
                ProcessGameAction(new AddUnit(unitDescription));
            }

            Loaded = true;
        }


    }

}

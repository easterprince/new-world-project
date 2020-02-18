﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletons;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using UnityEngine.Events;
using NewWorld.Utilities.Events;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController : ReloadableSingleton<UnitSystemController, List<UnitDescription>> {

        // Fields.

        // Structure.
        private long unusedUnitIndex = 0;
        private HashSet<UnitController> units = new HashSet<UnitController>();
        private Dictionary<UnitController, Vector2Int> positions = new Dictionary<UnitController, Vector2Int>();
        private Dictionary<Vector2Int, UnitController> onPositions = new Dictionary<Vector2Int, UnitController>();

        // Events.
        private ConditionalEvent<UnitController, Vector2Int> connectedNodeUpdatedEvent;
        private ConditionalEvent<UnitController> unitAddedEvent;
        private ConditionalEvent<UnitController, Vector2Int> unitRemovedEvent;


        // Properties.

        // Events.
        public UnityEvent<UnitController, Vector2Int> ConnectedNodeUpdatedEvent => connectedNodeUpdatedEvent;
        public UnityEvent<UnitController> UnitAddedEvent => unitAddedEvent;
        public UnityEvent<UnitController, Vector2Int> UnitRemovedEvent => unitRemovedEvent;


        // Life cycle.

        override private protected void Awake() {
            base.Awake();
            Instance = this;
            connectedNodeUpdatedEvent = new WhenLoadedEvent<UnitController, Vector2Int>(this);
            unitAddedEvent = new WhenLoadedEvent<UnitController>(this);
            unitRemovedEvent = new WhenLoadedEvent<UnitController, Vector2Int>(this);
        }

        private void Start() {
            MapController.EnsureInstance(this);
            MapController.Instance.UnloadedEvent.AddListener(() => StartReloading(null));
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
            Loaded = false;

            var removeUnits = new List<UnitController>(units);
            foreach (UnitController unit in removeUnits) {
                ProcessGameAction(new RemoveUnit(unit));
            }
            if (unitDescriptions != null) {
                foreach (UnitDescription unitDescription in unitDescriptions) {
                    ProcessGameAction(new AddUnit(unitDescription));
                }
            }

            Loaded = true;
        }


    }

}

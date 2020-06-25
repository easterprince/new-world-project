using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletons;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Unit.Actions.UnitUpdates;
using UnityEngine.Events;
using NewWorld.Utilities.Events;
using NewWorld.Battlefield.Unit;

namespace NewWorld.Battlefield.UnitSystem {

    public partial class UnitSystemController : ReloadableSingleton<UnitSystemController, List<UnitTemplate>>, IEnumerable<UnitController> {

        // Fields.

        // Structure.
        private readonly HashSet<UnitController> units = new HashSet<UnitController>();
        private readonly Dictionary<UnitController, Vector2Int> positions = new Dictionary<UnitController, Vector2Int>();
        private readonly Dictionary<Vector2Int, UnitController> onPositions = new Dictionary<Vector2Int, UnitController>();
        private readonly List<GameAction> unprocessedActions = new List<GameAction>();

        // Game objects.
#pragma warning disable IDE0044, CS0414, CS0649
        [SerializeField]
        private GameObject unitsGameObject;
#pragma warning restore IDE0044, CS0414, CS0649

        // Events.
        private ConditionalEvent<UnitController, Vector2Int> connectedNodeUpdatedEvent;
        private ConditionalEvent<UnitController> unitAddedEvent;
        private ConditionalEvent<UnitController, Vector2Int> unitRemovedEvent;


        // Properties.

        // Events.
        public UnityEvent<UnitController, Vector2Int> ConnectedNodeUpdatedEvent => connectedNodeUpdatedEvent;
        public UnityEvent<UnitController> UnitAddedEvent => unitAddedEvent;
        public UnityEvent<UnitController, Vector2Int> UnitRemovedEvent => unitRemovedEvent;


        // Enumerables.

        public IEnumerator<UnitController> GetEnumerator() {
            return ((IEnumerable<UnitController>) units).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable) units).GetEnumerator();
        }


        // Life cycle.

        override private protected void Awake() {
            base.Awake();
            if (unitsGameObject == null) {
                throw new MissingReferenceException($"Missing {unitsGameObject}.");
            }
            connectedNodeUpdatedEvent = new WhenLoadedEvent<UnitController, Vector2Int>(this);
            unitAddedEvent = new WhenLoadedEvent<UnitController>(this);
            unitRemovedEvent = new WhenLoadedEvent<UnitController, Vector2Int>(this);
        }

        private void Start() {
            MapController.EnsureInstance(this);
            MapController.Instance.UnloadedEvent.AddListener(StartClearing);
        }

        private void Update() {
            foreach (GameAction action in unprocessedActions) {
                ProcessGameAction(action);
            }
            unprocessedActions.Clear();
        }

        override private protected void OnDestroy() {
            MapController.Instance?.UnloadedEvent.RemoveListener(StartClearing);
            base.OnDestroy();
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

        public bool HasUnit(UnitController unit) {
            return units.Contains(unit);
        }


        // Reloading.

        override public void StartReloading(List<UnitTemplate> unitDescriptions) {
            Loaded = false;

            var removeUnits = new List<UnitController>(units);
            foreach (UnitController unit in removeUnits) {
                ProcessGameAction(new UnitRemoval(unit));
            }
            if (unitDescriptions != null) {
                foreach (UnitTemplate unitDescription in unitDescriptions) {
                    ProcessGameAction(new UnitAddition(unitDescription));
                }
            }

            Loaded = true;
        }


        // Action management.

        public void AddAction(GameAction gameAction) {
            if (gameAction == null) {
                throw new System.ArgumentNullException(nameof(gameAction));
            }
            unprocessedActions.Add(gameAction);
        }

        public void AddActions(IEnumerable<GameAction> gameActions) {
            if (gameActions == null) {
                throw new System.ArgumentNullException(nameof(gameActions));
            }
            foreach (var gameAction in gameActions) {
                AddAction(gameAction);
            }
        }


        // Event handlers.

        private void StartClearing() {
            StartReloading(null);
        }


    }

}

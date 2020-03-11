using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletons;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Abilities.Motions;
using NewWorld.Battlefield.Units.Abilities.Attacks;
using NewWorld.Battlefield.Units.Conditions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.General;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Units.Conditions.Collapses;
using NewWorld.Utilities;
using NewWorld.Battlefield.Units.Intelligence;

namespace NewWorld.Battlefield.Units {

    public partial class UnitController : MonoBehaviour {

        // Fabric.

        private const string prefabPath = "Prefabs/Unit";
        private const string defaultGameObjectName = "Unit";
        private static GameObject prefab;

        public static UnitController BuildUnit(Transform parent, UnitDescription description, string name = defaultGameObjectName) {
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject unit = Instantiate(prefab, new Vector3(-1, -1, -1), Quaternion.identity, parent);
            unit.name = name ?? defaultGameObjectName;
            GameObjects.SetLayerRecursively(unit, parent.gameObject.layer);
            unit.layer = parent.gameObject.layer;
            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.intelligence = new UnitIntelligence(unitController);
            unitController.durability = new UnitDurability(unitController, 100);
            unitController.ProcessGameActions(new GameAction[] {
                new AttachAbility(unitController, new BasicMotion(2)),
                new AttachAbility(unitController, new BasicAttack(20, 2))
            }, true);
            return unitController;
        }


        // Fields.

        // Gameobject components.
        private Animator animator;
        new private Collider collider;

        // Game logic modules.
        private UnitIntelligence intelligence = null;
        private UnitDurability durability = null;
        private Condition currentCondition = null;
        private readonly HashSet<Ability> abilities = new HashSet<Ability>();

        // Module passport.
        private ParentPassport<UnitController> ownPassport;

        // Actions.
        private readonly List<GameAction> unprocessedActions = new List<GameAction>();


        // Properties.

        public Vector3 Position {
            get {
                if (this == null) {
                    throw new System.InvalidOperationException("Unit is destroyed, property is invalid.");
                }
                return transform.position;
            }
        }

        public Quaternion Rotation {
            get {
                if (this == null) {
                    throw new System.InvalidOperationException("Unit is destroyed, property is invalid.");
                }
                return transform.rotation;
            }
        }

        public Collider ColliderComponent {
            get {
                if (this == null) {
                    throw new System.InvalidOperationException("Unit is destroyed, property is invalid.");
                }
                return collider;
            }
        }

        public bool Collapsing {
            get {
                if (durability != null) {
                    return durability.Broken;
                }
                return false;
            }
        }

        public UnitIntelligence Intelligence => intelligence;
        public UnitDurability Durability => durability;
        public Condition CurrentCondition => currentCondition;
        public ICollection<Ability> Abilities {
            get {
                var abilityPresentations = new Ability[abilities.Count];
                abilities.CopyTo(abilityPresentations, 0);
                return abilityPresentations;
            }
        }


        // Informational methods.

        public TAbility GetAbility<TAbility>()
            where TAbility : Ability {

            foreach (var ability in abilities) {
                if (ability is TAbility found) {
                    return found;
                }
            }

            return default;
        }


        // Life cycle.

        private void Awake() {
            UnitSystemController.EnsureInstance(this);
            MapController.EnsureInstance(this);
            animator = GetComponent<Animator>();
            GameObjects.ValidateComponent(animator);
            collider = GetComponent<Collider>();
            GameObjects.ValidateComponent(collider);
            ownPassport = new ParentPassport<UnitController>(this);
        }

        private void Start() {
            SetDefaultLocation();
        }

        private void Update() {

            // Process unprocessed actions.
            foreach (var gameAction in unprocessedActions) {
                ProcessGameAction(gameAction, true);
            }
            unprocessedActions.Clear();

            // Ask behaviour for orders.
            if (!Collapsing) {
                if (intelligence != null) {
                    intelligence.Act(ownPassport, out CancelCondition cancelCondition, out UseAbility useAbility);
                    if (cancelCondition != null) {
                        ProcessGameAction(cancelCondition, false);
                    }
                    if (useAbility != null) {
                        ProcessGameAction(useAbility, false);
                    }
                }
            }

            // Receive and process actions from used ability.
            if (currentCondition != null) {
                var actions = currentCondition.Update(ownPassport);
                if (currentCondition.Exited) {
                    currentCondition = null;
                }
                ProcessGameActions(actions, false);
            }

            // Change condition to collapsing.
            if (Collapsing) {
                if (!(currentCondition is CollapseCondition)) {
                    var collapseCondition = new SimpleCollapse(2);
                    var forceCondition = new ForceCondition(this, collapseCondition);
                    ProcessGameAction(forceCondition, false);
                }
            }

        }


        // Actions management and used ability update.

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


        // Support methods.

        private void SetDefaultLocation() {
            Vector2Int connectedNode = UnitSystemController.Instance.GetConnectedNode(this);
            float height = MapController.Instance.GetSurfaceHeight(connectedNode);
            transform.position = new Vector3(connectedNode.x, height, connectedNode.y);
        }


    }

}
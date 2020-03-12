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

        private static GameObject prefab;
        private static int builtUnits = 0;

        public static UnitController BuildUnit(Transform parent, UnitDescription description) {
            if (parent == null) {
                throw new System.ArgumentNullException(nameof(parent));
            }

            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }

            GameObject unit = Instantiate(prefab, parent);
            unit.name = $"Unit {builtUnits + 1}";
            ++builtUnits;
            GameObjects.SetLayerRecursively(unit, parent.gameObject.layer);
            
            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.intelligence = new UnitIntelligence();
            unitController.intelligence.Connect(unitController.ownPassport);
            unitController.durability = new UnitDurability(100);
            unitController.durability.Connect(unitController.ownPassport);
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
        private UnitCondition currentCondition = null;
        private readonly HashSet<UnitAbility> abilities = new HashSet<UnitAbility>();

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

        public UnitIntelligence Intelligence => intelligence;
        public UnitDurability Durability => durability;
        public UnitCondition CurrentCondition => currentCondition;
        public ICollection<UnitAbility> Abilities {
            get {
                var abilityPresentations = new UnitAbility[abilities.Count];
                abilities.CopyTo(abilityPresentations, 0);
                return abilityPresentations;
            }
        }


        // Informational methods.

        public TAbility GetAbility<TAbility>()
            where TAbility : UnitAbility {

            foreach (var ability in abilities) {
                if (ability is TAbility found) {
                    return found;
                }
            }

            return default;
        }


        // Life cycle.

        private void Awake() {
            animator = GetComponent<Animator>();
            GameObjects.ValidateComponent(animator);
            collider = GetComponent<Collider>();
            GameObjects.ValidateComponent(collider);
            ownPassport = new ParentPassport<UnitController>(this);
        }

        private void Start() {
            UnitSystemController.EnsureInstance(this);
            MapController.EnsureInstance(this);
            if (!UnitSystemController.Instance.HasUnit(this)) {
                throw new System.Exception("Unit system is unaware of this unit!");
            }
            SetDefaultLocation();
        }

        private void Update() {

            // Process unprocessed actions.
            foreach (var gameAction in unprocessedActions) {
                ProcessGameAction(gameAction, true);
            }
            unprocessedActions.Clear();

            // Update on durability.
            if (durability != null) {
                durability.Update(ownPassport, out ForceCondition forceCondition);
                if (forceCondition != null) {
                    ProcessGameAction(forceCondition, false);
                }
            }

            // Ask behaviour for orders.
            if (intelligence != null) {
                intelligence.Act(ownPassport, out CancelCondition cancelCondition, out UseAbility useAbility);
                if (cancelCondition != null) {
                    ProcessGameAction(cancelCondition, false);
                }
                if (useAbility != null) {
                    ProcessGameAction(useAbility, false);
                }
            }

            // Receive and process actions from used ability.
            if (currentCondition != null) {
                var actions = currentCondition.Update(ownPassport);
                if (currentCondition.Status == UnitCondition.StatusType.Exited) {
                    currentCondition.Disconnect(ownPassport);
                    currentCondition = null;
                }
                ProcessGameActions(actions, false);
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
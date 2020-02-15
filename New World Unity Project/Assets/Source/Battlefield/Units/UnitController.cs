using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Behaviours;
using NewWorld.Battlefield.Units.Abilities.Motions;
using NewWorld.Battlefield.Units.Abilities.Attacks;
using NewWorld.Battlefield.Units.Conditions;
using NewWorld.Battlefield.Units.Actions.UnitUpdates.General;
using NewWorld.Battlefield.Units.Actions.UnitSystemUpdates;
using NewWorld.Battlefield.Units.Conditions.Collapses;

namespace NewWorld.Battlefield.Units {

    public partial class UnitController : MonoBehaviour, IActing {

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
            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.behaviour = new UnitBehaviour(unitController);
            unitController.motionAbility = new BasicMotion(unitController, 2);
            unitController.attackAbility = new BasicAttack(unitController, 20, 2);
            unitController.durability = new UnitDurability(unitController, 100);
            return unitController;
        }


        // Static.

        private const float nodeDistanceLimit = 0.6f;

        public static float NodeDistanceLimit => nodeDistanceLimit;


        // Fields.

        // Gameobject components.
        private Animator animator;

        // Game logic components.
        private UnitBehaviour behaviour = null;
        private UnitDurability durability = null;
        private MotionAbility motionAbility = null;
        private AttackAbility attackAbility = null;

        // Actions.
        private List<GameAction> actionsToReturn = new List<GameAction>();

        // Conditions.
        private Condition currentCondition = null;


        // Properties.

        public MotionAbility MotionAbility => motionAbility;
        public AttackAbility AttackAbility => attackAbility;
        public Condition CurrentCondition => currentCondition;

        public bool Collapsed {
            get {
                if (durability != null) {
                    return durability.Collapsed;
                }
                return false;
            }
        }

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;


        // Informational methods.

        public bool HasAbility(Ability ability) {
            if (ability == null) {
                return false;
            }
            return motionAbility == ability || attackAbility == ability;
        }


        // Life cycle.

        private void Awake() {
            if (UnitSystemController.Instance == null) {
                throw new MissingSingletonException<UnitSystemController>(this);
            }
            if (MapController.Instance == null) {
                throw new MissingSingletonException<MapController>(this);
            }
            animator = GetComponent<Animator>();
        }

        private void Start() {
            SetDefaultLocation();
        }

        private void Update() {

            if (!Collapsed) {

                // Ask behaviour for orders.
                if (behaviour != null) {
                    behaviour.Act(out CancelCondition cancelCondition, out UseAbility useAbility);
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
                var actions = currentCondition.Update();
                if (currentCondition.Exited) {
                    currentCondition = null;
                }
                ProcessGameActions(actions, false);
            }

            if (Collapsed) {

                // Change condition to collapse.
                if (!(currentCondition is CollapseCondition)) {
                    var collapseCondition = new SimpleCollapse(this, 2);
                    var forceCondition = new ForceCondition(collapseCondition);
                    ProcessGameAction(forceCondition, false);
                }

            }

        }


        // Actions management and used ability update.

        public IEnumerable<GameAction> ReceiveActions() {
            var actions = actionsToReturn;
            actionsToReturn = new List<GameAction>();
            return actions;
        }


        // Support methods.

        private void SetDefaultLocation() {
            Vector2Int connectedNode = UnitSystemController.Instance.GetConnectedNode(this);
            float height = MapController.Instance.GetSurfaceHeight(connectedNode);
            transform.position = new Vector3(connectedNode.x, height, connectedNode.y);
        }


    }

}
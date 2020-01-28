using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Behaviours;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Abilities.Active;
using NewWorld.Battlefield.Units.Abilities.Active.Motions;
using NewWorld.Battlefield.Units.Abilities.Active.Attacks;

namespace NewWorld.Battlefield.Units {

    public partial class UnitController : MonoBehaviour, IActing {

        // Fabric.

        private const string prefabPath = "Prefabs/Unit";
        private const string defaultGameObjectName = "Unit";
        private static GameObject prefab;

        public static UnitController BuildUnit(UnitDescription description, string name = defaultGameObjectName) {
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject unit = Instantiate(prefab, new Vector3(-1, -1, -1), Quaternion.identity);
            unit.name = name ?? defaultGameObjectName;
            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.behaviour = new UnitBehaviour(unitController);
            unitController.motionAbility = new BasicMotion(unitController);
            unitController.attackAbility = new BasicAttack(unitController);
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
        private BasicMotion motionAbility = null;
        private BasicAttack attackAbility = null;

        // Actions.
        private List<GameAction> actionsToReturn = new List<GameAction>();

        // Ability using.
        private ActiveAbility usedAbility = null;
        private AbilityUsage plannedAbilityUsage = null;
        private AbilityStop plannedAbilityStop = null;


        // Properties.

        public UnitBehaviour Behaviour {
            get => behaviour;
        }

        public BasicMotion MotionAbility {
            get => motionAbility;
        }

        public BasicAttack AttackAbility {
            get => attackAbility;
        }

        public Ability UsedAbility {
            get => usedAbility;
        }

        public bool Broken {
            get {
                if (durability != null) {
                    return durability.Broken;
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


        // Actions management and used ability update.

        public IEnumerable<GameAction> ReceiveActions() {

            void CheckUsageAndProcessActions(IEnumerable<GameAction> actions) {
                if (!usedAbility.IsUsed) {
                    usedAbility = null;
                }
                foreach (GameAction action in actions) {
                    if (!ProcessGameAction(action)) {
                        actionsToReturn.Add(action);
                    }
                }
            }

            // Ask behaviour for orders.
            if (behaviour != null) {
                behaviour.Act(out AbilityCancellation abilityCancellation, out AbilityUsage abilityUsage);
                if (abilityCancellation != null) {
                    ProcessUnitUpdate(abilityCancellation);
                }
                if (abilityUsage != null) {
                    ProcessUnitUpdate(abilityUsage);
                }
            }

            // Update used ability.
            if (plannedAbilityStop != null) {
                if (plannedAbilityStop.Ability == usedAbility) {
                    var actions = usedAbility.Stop(plannedAbilityStop.ForceStop);
                    CheckUsageAndProcessActions(actions);
                }
                plannedAbilityStop = null;
            }
            if (plannedAbilityUsage != null) {
                if (HasAbility(plannedAbilityUsage.Ability)) {
                    usedAbility = plannedAbilityUsage.Ability;
                    var actions = usedAbility.Use(plannedAbilityUsage.ParameterSet);
                    CheckUsageAndProcessActions(actions);
                }
                plannedAbilityUsage = null;
            }

            // Receive and process actions from used ability.
            if (usedAbility != null) {
                var actions = usedAbility.ReceiveActions();
                CheckUsageAndProcessActions(actions);
            }

            var unprocessedActions = actionsToReturn;
            actionsToReturn = new List<GameAction>();
            return unprocessedActions;
        }


        // Support methods.

        private void SetDefaultLocation() {
            Vector2Int connectedNode = UnitSystemController.Instance.GetConnectedNode(this);
            float height = MapController.Instance.GetSurfaceHeight(connectedNode);
            transform.position = new Vector3(connectedNode.x, height, connectedNode.y);
        }


    }

}
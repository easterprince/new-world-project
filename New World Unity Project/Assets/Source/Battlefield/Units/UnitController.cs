using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Behaviours;
using NewWorld.Battlefield.Units.Actions.UnitUpdates;
using NewWorld.Battlefield.Units.Abilities.Active.Motion;
using NewWorld.Battlefield.Units.Abilities.Active;

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
            unitController.motionAbility = new SimpleMotion(unitController);
            return unitController;
        }


        // Fields.

        // Gameobject components.
        private Animator animator;

        // Game logic components.
        private UnitBehaviour behaviour = null;
        private MotionAbility motionAbility = null;
        private float health = 1;

        // Actions.
        private List<GameAction> exteriorActions = new List<GameAction>();

        // Ability using.
        private ActiveAbility usedAbility = null;
        private AbilityUsage plannedAbilityUsage = null;
        private AbilityStop plannedAbilityStop = null;


        // Properties.

        public UnitBehaviour Behaviour {
            get => behaviour;
        }

        public MotionAbility MotionAbility {
            get => motionAbility;
        }

        public Ability UsedAbility {
            get => usedAbility;
        }

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;


        // Informational methods.

        public bool HasAbility(Ability ability) {
            if (ability == null) {
                return false;
            }
            return motionAbility == ability;
        }


        // Life cycle.

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void Start() {
            SetDefaultLocation();
        }


        // Actions management.

        public IEnumerable<GameAction> ReceiveActions() {

            void CheckUsageAndProcessActions(IEnumerable<GameAction> actions) {
                if (!usedAbility.IsUsed) {
                    usedAbility = null;
                }
                foreach (GameAction action in actions) {
                    if (!ProcessGameAction(action)) {
                        exteriorActions.Add(action);
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

            var unprocessedActions = exteriorActions;
            exteriorActions = new List<GameAction>();
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
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

        // Parameters.
        private ActiveAbility usedAbility = null;
        private float health = 1;


        // Properties.

        public UnitBehaviour Behaviour {
            get => behaviour;
        }

        public Ability UsedAbility {
            get => usedAbility;
        }

        public MotionAbility MotionAbility {
            get => motionAbility;
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
            
            // Update used ability.
            if (behaviour != null) {
                AbilityUsage abilityUsage = behaviour.Act();
                if (abilityUsage != null) {
                    ProcessUnitUpdate(abilityUsage);
                }
            }

            // Receive and process actions from used ability.
            List<GameAction> actions = null;
            if (usedAbility != null) {
                foreach (GameAction action in usedAbility.ReceiveActions()) {
                    if (action is UnitUpdate unitUpdate && unitUpdate.UpdatedUnit == this && ProcessUnitUpdate(unitUpdate)) {
                        continue;
                    }
                    if (actions == null) {
                        actions = new List<GameAction>();
                    }
                    actions.Add(action);
                }
                if (!usedAbility.IsUsed) {
                    usedAbility = null;
                }
            }

            if (actions == null) {
                return Enumerables.GetNothing<GameAction>();
            }
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
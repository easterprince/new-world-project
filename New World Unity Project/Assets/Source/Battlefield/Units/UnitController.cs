using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Behaviours;

namespace NewWorld.Battlefield.Units {

    public class UnitController : MonoBehaviour, IActing {

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
        private float health = 1;


        // Properties.

        public Animator Animator => animator;

        public UnitBehaviour Behaviour {
            get => behaviour;
        }

        public MotionAbility MotionAbility {
            get => motionAbility;
        }

        public IEnumerable<Ability> AllAbilities {
            get {
                if (motionAbility != null) {
                    yield return motionAbility;
                }
            }
        }

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;


        // Life cycle.

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void Start() {
            SetDefaultLocation();
        }


        // Actions management.

        public IEnumerable<GameAction> ReceiveActions() {
            if (behaviour != null) {
                behaviour.Act();
            }
            List<GameAction> actions = null;
            foreach (Ability ability in AllAbilities) {
                foreach (GameAction action in ability.ReceiveActions()) {
                    if (action is UnitUpdate unitUpdate && unitUpdate.UpdatedUnit == this) {
                        // TODO: Implement overloaded methods to process different types of actions.
                        if (unitUpdate is TransformUpdate transformUpdate) {
                            if (transformUpdate.NewPosition != null) {
                                transform.position = transformUpdate.NewPosition.Value;
                            }
                            if (transformUpdate.NewRotation != null) {
                                transform.rotation = transformUpdate.NewRotation.Value;
                            }
                            continue;
                        }
                    }
                    if (actions == null) {
                        actions = new List<GameAction>();
                    }
                    actions.Add(action);
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
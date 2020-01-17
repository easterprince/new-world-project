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

        // Components.
        private UnitBehaviour behaviour = null;
        private MotionAbility motionAbility = null;

        // Parameters.
        private float health = 1;


        // Properties.

        public UnitBehaviour Behaviour {
            get => behaviour;
            set => behaviour = value;
        }

        public MotionAbility MotionAbility {
            get => motionAbility;
            set => motionAbility = value;
        }

        public IEnumerable<Ability> AllAbilities {
            get {
                if (motionAbility != null) {
                    yield return motionAbility;
                }
            }
        }

        public Vector3 Position => transform.position;


        // Life cycle.

        private void Update() {
            UpdatePosition();
        }


        // Updates.

        private void UpdatePosition() {
            if (motionAbility.Moves) {
                transform.position = motionAbility.UpdatePosition();
            } else {
                Vector2Int connectedNode = UnitSystemController.Instance.GetConnectedNode(this);
                float height = MapController.Instance.GetSurfaceHeight(connectedNode);
                transform.position = new Vector3(connectedNode.x, height, connectedNode.y);
            }
        }


        // Actions generating.

        public IEnumerable<UnitAction> ReceiveActions() {
            if (behaviour != null) {
                behaviour.Act();
            }
            List<UnitAction> actions = null;
            foreach (Ability ability in AllAbilities) {
                foreach (UnitAction action in ability.ReceiveActions()) {
                    if (actions == null) {
                        actions = new List<UnitAction>();
                    }
                    actions.Add(action);
                }
            }
            if (actions == null) {
                return Enumerables.GetNothing<UnitAction>();
            }
            return actions;
        }


    }

}
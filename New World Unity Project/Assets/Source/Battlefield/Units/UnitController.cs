using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Intentions;
using NewWorld.Battlefield.Units.Core;

namespace NewWorld.Battlefield.Units {

    public class UnitController : MonoBehaviour, IIntending {

        // Fabric.

        private const string prefabPath = "Prefabs/Unit";
        private const string defaultGameObjectName = "Unit";
        private static GameObject prefab;

        public static UnitController BuildUnit(UnitDescription description, string name = defaultGameObjectName) {
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject unit = Instantiate(prefab);
            unit.name = name ?? defaultGameObjectName;
            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.core = new UnitCore(description);
            return unitController;
        }


        // Constants.

        private const float unitSize = 0.49f;


        // Fields.

        private UnitCore core;


        // Properties.

        public Vector2Int ConnectedNode => core.ConnectedNode;


        // Life cycle.

        private void Awake() {}

        private void Update() {
            transform.position = core.GetPosition();
        }


        // Outer control.

        public IEnumerable<Intention> ReceiveIntentions() {
            return core.ReceiveIntentions();
        }

        public void Fulfil(Intention intention) {
            core.Fulfil(intention);
        }


    }

}
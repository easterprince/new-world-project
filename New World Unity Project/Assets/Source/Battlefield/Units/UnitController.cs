using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Composition;
using NewWorld.Battlefield.Units.Intentions;
using NewWorld.Battlefield.Units.Core;

namespace NewWorld.Battlefield.Units {

    public class UnitController : MonoBehaviour, IIntending {

        // Fabric.

        private const string prefabPath = "Prefabs/Unit";
        private const string defaultGameObjectName = "Unit";
        private static GameObject prefab;

        public static UnitController BuildUnit(UnitDescription description, int visionDirection, string name = defaultGameObjectName) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject unit = Instantiate(prefab);
            unit.name = name ?? defaultGameObjectName;
            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.core = new UnitCore(description);
            unitController.currentVisionDirection = visionDirection;
            return unitController;
        }


        // Constants.

        private const float unitSize = 0.49f;


        // Fields.

        private int currentVisionDirection;
        private UnitCore core;

        private SpriteRenderer spriteRenderer;


        // Properties.

        public Vector2Int ConnectedNode => core.ConnectedNode;


        // Life cycle.

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            UpdateVisiblePosition();
        }


        // Outer control.

        public void Rotate(int visionDirection) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            currentVisionDirection = visionDirection;
            UpdateVisiblePosition();
        }

        public IEnumerable<Intention> ReceiveIntentions() {
            return core.ReceiveIntentions();
        }

        public void Fulfil(Intention intention) {
            core.Fulfil(intention);
        }


        // Support.

        private void UpdateVisiblePosition() {
            Vector3 realPosition = core.GetPosition();
            transform.position = CoordinatesTransformations.RealToVisible(realPosition, currentVisionDirection, unitSize, out int spriteOrder);
            spriteRenderer.sortingOrder = spriteOrder + (int) SpriteLayers.Sublayers.Units;
        }
    }

}
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Composition;

namespace NewWorld.Battlefield.Units {

    public class UnitController : MonoBehaviour {

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
            unitController.description = new UnitDescription(description);
            unitController.currentVisionDirection = visionDirection;
            return unitController;
        }


        // Fields.

        private UnitDescription description;
        private int currentVisionDirection;

        private SpriteRenderer spriteRenderer;


        // Properties.

        public UnitDescription Description => description;


        // Life cycle.

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start() {
            UpdateVisiblePosition();
        }


        // Controlling methods.

        public void Rotate(int visionDirection) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            currentVisionDirection = visionDirection;
            UpdateVisiblePosition();
        }


        // Support.

        private void UpdateVisiblePosition() {
            float height = MapController.Instance.GetSurfaceHeight(description.CurrentNode);
            Vector3 realPosition = new Vector3(description.CurrentNode.x, description.CurrentNode.y, height);
            transform.position = CoordinatesTransformations.RealToVisible(realPosition, currentVisionDirection, out int spriteOrder);
            spriteRenderer.sortingOrder = spriteOrder;
        }


    }

}
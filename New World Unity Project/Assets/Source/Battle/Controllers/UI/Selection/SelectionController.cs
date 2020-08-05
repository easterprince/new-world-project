using NewWorld.Battle.Controllers.Cameras;
using NewWorld.Battle.Controllers.Unit;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battle.Controllers.UI.Selection {

    public class SelectionController : SteadyController {

        // Fields.

        private UnitController selected;

        // References.
        [SerializeField]
        private CameraController mainCamera;

        // Components.
        private Image image;


        // Properties.

        public CameraController MainCamera {
            get => mainCamera;
            set {
                ValidateBeingNotStarted();
                mainCamera = value;
            }
        }

        public UnitController Selected {
            get => selected;
            set {
                selected = value;
                gameObject.SetActive(selected != null);
            }
        }


        // Life cycle.

        private protected override void OnStart() {
            GameObjects.ValidateReference(mainCamera, nameof(mainCamera));
            image = GetComponent<Image>();
            GameObjects.ValidateComponent(image);
        }

        private void LateUpdate() {
            if (selected == null) {
                Selected = null;
                return;
            }
            CalculateSizeOnScreen(out Vector2 position, out float size);
            transform.position = position;
            transform.localScale = new Vector3(size, size, 1);
        }


        // Support.

        private void CalculateSizeOnScreen(out Vector2 position, out float size) {
            Camera camera = mainCamera.Camera;
            Collider collider = selected.Collider;
            if (camera == null || collider == null) {
                position = Vector2.zero;
                size = 0;
                return;
            }
            Bounds bounds = collider.bounds;

            // Calculate position.
            Vector3 center = bounds.center;
            Vector3 centerProjection = camera.WorldToScreenPoint(center);
            position = centerProjection;

            // Calculate size.
            Vector3 extents = bounds.extents;
            float radius = 0;
            for (int mask = 0; mask < 1 << 3; ++mask) {

                // Calculate edge.
                Vector3 addition = extents;
                if ((mask & 1 << 0) != 0) {
                    addition.x *= -1;
                }
                if ((mask & 1 << 1) != 0) {
                    addition.y *= -1;
                }
                if ((mask & 1 << 2) != 0) {
                    addition.z *= -1;
                }
                Vector3 edge = center + addition;

                // Check if edge is visible.
                Vector3 edgeOnViewport = camera.WorldToViewportPoint(edge);
                if (edgeOnViewport.x < 0 || edgeOnViewport.x > 1 || edgeOnViewport.y < 0 || edgeOnViewport.y > 1 || edgeOnViewport.z < 0) {
                    continue;
                }

                // Update radius.
                Vector3 edgeProjection = camera.WorldToScreenPoint(edge);
                float localRadius = (edgeProjection - centerProjection).magnitude;
                radius = Mathf.Max(radius, localRadius);

            }
            size = 2 * radius;

        }


    }

}

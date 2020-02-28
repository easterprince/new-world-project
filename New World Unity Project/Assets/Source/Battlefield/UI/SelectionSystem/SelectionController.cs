using NewWorld.Battlefield.Units;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battlefield.UI.SelectionSystem {
    
    public class SelectionController : MonoBehaviour {

        // Fields.

        private Image image;

        private UnitController selected;


        // Properties.

        public UnitController Selected {
            get => selected;
            set {
                selected = value;
                gameObject.SetActive(selected != null);
                image.enabled = false;
            }
        }


        // Life cycle.

        private void Start() {
            image = GetComponent<Image>() ?? throw new MissingComponentException("Need Image component!");
        }

        private void Update() {
            if (selected == null) {
                Selected = null;
                return;
            }
            if (!image.enabled) {
                image.enabled = true;
            }
            CalculateScreenSize(out Vector2 position, out float size);
            transform.position = position;
            transform.localScale = new Vector3(size, size, 1);
        }


        // Support.

        private void CalculateScreenSize(out Vector2 position, out float size) {
            Camera cameraComponent = BattlefieldCameraController.Instance.CameraComponent;
            Bounds bounds = selected.ColliderComponent.bounds;

            // Calculate position.
            Vector3 center = bounds.center;
            Vector3 centerProjection = cameraComponent.WorldToScreenPoint(center);
            position = centerProjection;

            // Calculate size.
            Vector3 extents = bounds.extents;
            float radius = 0;
            for (int mask = 0; mask < (1 << 3); ++mask) {

                // Calculate edge.
                Vector3 addition = extents;
                if ((mask & (1 << 0)) != 0) {
                    addition.x *= -1;
                }
                if ((mask & (1 << 1)) != 0) {
                    addition.y *= -1;
                }
                if ((mask & (1 << 2)) != 0) {
                    addition.z *= -1;
                }
                Vector3 edge = center + addition;

                // Check if edge is visible.
                Vector3 edgeOnViewport = cameraComponent.WorldToViewportPoint(edge);
                if (edgeOnViewport.x < 0 || edgeOnViewport.x > 1 || edgeOnViewport.y < 0 || edgeOnViewport.y > 1 || edgeOnViewport.z < 0) {
                    continue;
                }

                // Update radius.
                Vector3 edgeProjection = cameraComponent.WorldToScreenPoint(edge);
                float localRadius = (edgeProjection - centerProjection).magnitude;
                radius = Mathf.Max(radius, localRadius);

            }
            size = 2 * radius;

        }


    }

}

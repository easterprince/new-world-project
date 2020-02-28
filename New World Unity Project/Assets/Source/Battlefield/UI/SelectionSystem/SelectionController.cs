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
            Bounds bounds = selected.ColliderComponent.bounds;
            Vector3 center = bounds.center;
            Vector2 centerProjection = BattlefieldCameraController.Instance.CameraComponent.WorldToScreenPoint(center);
            Vector3 extents = bounds.extents;
            float radius = 0;
            for (int mask = 0; mask < (1 << 2); ++mask) {
                Vector3 addition = extents;
                if ((mask & (1 << 0)) != 0) {
                    addition.x *= -1;
                }
                if ((mask & (1 << 1)) != 0) {
                    addition.y *= -1;
                }
                Vector3 edge = center + addition;
                Vector2 edgeProjection = BattlefieldCameraController.Instance.CameraComponent.WorldToScreenPoint(edge);
                radius = Mathf.Max(radius, (edgeProjection - centerProjection).magnitude);
            }
            position = centerProjection;
            size = 2 * radius;
        }


    }

}

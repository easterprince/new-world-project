using NewWorld.Battle.Controllers.Cameras;
using NewWorld.Battle.Controllers.Unit;
using NewWorld.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NewWorld.Battle.Controllers.UI.UnitPanel {

    public class UnitPortraitController : MonoBehaviour, IPointerClickHandler {

        // Fields.

        private UnitController followed;

        // Steady references.
        [SerializeField]
        private CameraController mainCamera;
        [SerializeField]
        private CameraController portraitCamera;
        [SerializeField]
        private GameObject cameraView;


        // Properties.

        public UnitController Followed {
            get => followed;
            set => followed = value;
        }


        // Life cycle.

        private void Start() {
            GameObjects.ValidateReference(mainCamera, nameof(mainCamera));
            GameObjects.ValidateReference(portraitCamera, nameof(portraitCamera));
            GameObjects.ValidateReference(cameraView, nameof(cameraView));
        }

        private void LateUpdate() {
            if (followed == null || followed.Collider == null) {
                if (cameraView.activeSelf) {
                    cameraView.SetActive(false);
                }
            } else {
                if (!cameraView.activeSelf) {
                    cameraView.SetActive(true);
                }
                var location = new CameraLocation() {
                    ViewedPosition = followed.Collider.bounds.center,
                    ViewingDistance = 2,
                    Rotation = mainCamera.CurrentLocation.Rotation
                };
                portraitCamera.CurrentLocation = location;
            }
        }


        // Event handlers.

        public void OnPointerClick(PointerEventData eventData) {

            // Process camera relocation.
            if (eventData.button == PointerEventData.InputButton.Left) {
                if (followed == null) {
                    return;
                }
                var location = mainCamera.CurrentLocation;
                location.ViewedPosition = followed.Center;
                mainCamera.CurrentLocation = location;
            }

        }


    }

}

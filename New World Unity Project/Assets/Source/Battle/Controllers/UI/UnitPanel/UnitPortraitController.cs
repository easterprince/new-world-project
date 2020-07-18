using NewWorld.Battle.Controllers.Cameras;
using NewWorld.Battle.Controllers.Unit;
using NewWorld.Utilities;
using NewWorld.Utilities.Events;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NewWorld.Battle.Controllers.UI.UnitPanel {

    public class UnitPortraitController : MonoBehaviour, IPointerClickHandler {

        // Fields.

        private UnitController followed;

        // Steady references.
        [SerializeField]
        private CameraController portraitCamera;
        [SerializeField]
        private GameObject cameraView;

        // Events.
        private readonly ControllerEvent clickEvent = new ControllerEvent();


        // Properties.

        public ControllerEvent.EventWrapper ClickEvent => clickEvent.Wrapper;

        public UnitController Followed {
            get => followed;
            set => followed = value;
        }


        // Life cycle.

        private void Start() {
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
                var location = portraitCamera.CurrentLocation;
                location.ViewedPosition = followed.Collider.bounds.center;
                location.ViewingDistance = 2;
                portraitCamera.CurrentLocation = location;
            }
        }


        // Event handlers.

        public void OnPointerClick(PointerEventData eventData) {
            clickEvent.Invoke();
        }


    }

}

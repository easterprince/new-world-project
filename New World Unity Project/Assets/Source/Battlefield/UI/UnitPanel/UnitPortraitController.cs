using NewWorld.Battlefield.Cameras;
using NewWorld.Battlefield.Units;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NewWorld.Battlefield.UI.UnitPanel {
    
    public class UnitPortraitController : MonoBehaviour, IPointerClickHandler {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private CameraController portraitCamera;

        [SerializeField]
        private GameObject cameraView;

#pragma warning restore IDE0044, CS0414, CS0649

        private UnityEvent pointerClickEvent;

        private UnitController followed;


        // Properties.

        public UnityEvent PointerClickEvent => pointerClickEvent;

        public UnitController Followed {
            get => followed;
            set => followed = value;
        }


        // Life cycle.

        private void Awake() {
            pointerClickEvent = new UnityEvent();
        }

        private void Start() {
            if (portraitCamera == null) {
                throw new MissingReferenceException("Missing portrait camera!");
            }
            if (cameraView == null) {
                throw new MissingReferenceException("Missing camera view!");
            }
        }

        private void Update() {
            if (followed == null) {
                if (cameraView.activeSelf) {
                    cameraView.SetActive(false);
                }
            } else {
                if (!cameraView.activeSelf) {
                    cameraView.SetActive(true);
                }
                portraitCamera.Center(followed);
                portraitCamera.CurrentViewingDistance = 2;
            }
        }


        // Event handlers.

        public void OnPointerClick(PointerEventData eventData) {
            pointerClickEvent.Invoke();
        }


    }

}

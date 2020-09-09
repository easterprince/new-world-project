using NewWorld.Controllers.Battle.Cameras;
using NewWorld.Controllers.Battle.Map;
using NewWorld.Controllers.Battle.Unit;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using NewWorld.Utilities.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NewWorld.Controllers.Battle.UI.Selection {

    public class SelectionSystemController : SteadyController, IPointerClickHandler {

        // Event types.

        public class UnitEvent : ControllerEvent<UnitController> {}
        public class PositionEvent : ControllerEvent<Vector3> {}


        // Fields.

        private UnitController mainSelected = null;

        // Events.
        private readonly UnitEvent unitSelectedEvent = new UnitEvent();
        private readonly UnitEvent unitTargetedEvent = new UnitEvent();
        private readonly PositionEvent positionTargetedEvent = new PositionEvent();

        // Steady references.
        [SerializeField]
        private SelectionController mainSelection;
        [SerializeField]
        private CameraController mainCamera;


        // Properties.

        public UnitController MainSelected => mainSelected;

        public UnitEvent.EventWrapper UnitSelectedEvent => unitSelectedEvent.Wrapper;
        public UnitEvent.EventWrapper UnitTargetedEvent => unitTargetedEvent.Wrapper;
        public PositionEvent.EventWrapper PositionTargetedEvent => positionTargetedEvent.Wrapper;

        public SelectionController MainSelection {
            get => mainSelection;
            set {
                ValidateBeingNotStarted();
                mainSelection = value;
            }
        }

        public CameraController MainCamera {
            get => mainCamera;
            set {
                ValidateBeingNotStarted();
                mainCamera = value;
            }
        }


        // Life cycle.

        private protected override void OnStart() {
            GameObjects.ValidateReference(mainSelection, nameof(mainSelection));
            GameObjects.ValidateReference(mainCamera, nameof(mainCamera));
        }

        private void OnDestroy() {
            unitSelectedEvent.Clear();
            unitTargetedEvent.Clear();
            positionTargetedEvent.Clear();
        }


        // Pointer event handlers.

        void IPointerClickHandler.OnPointerClick(PointerEventData pointerEventData) {

            // Evaluate clicked object.
            var pointerPosition = pointerEventData.pressPosition;
            var pointerRay = mainCamera.Camera.ScreenPointToRay(pointerPosition);
            var layerMask = LayerMask.GetMask("Units", "Terrain");
            Physics.Raycast(pointerRay, out RaycastHit raycastHit, float.PositiveInfinity, layerMask);
            var colliderHit = raycastHit.collider;
            GameObject hit = null;
            if (colliderHit != null) {
                hit = colliderHit.transform.gameObject;
            }

            // Process pointer click.
            if (pointerEventData.button == PointerEventData.InputButton.Left) {
                if (hit == null) {
                    ChangeSelection(null);
                } else {
                    var unit = hit.GetComponent<UnitController>();
                    if (unit == null) {
                        ChangeSelection(null);
                    } else {
                        ChangeSelection(unit);
                    }
                }
            } else if (pointerEventData.button == PointerEventData.InputButton.Right) {
                if (hit != null) {
                    var unit = hit.GetComponent<UnitController>();
                    if (unit != null) {
                        SetTarget(unit);
                    }
                    var cluster = hit.GetComponent<ClusterController>();
                    if (cluster != null) {
                        SetTarget(raycastHit.point);
                    }
                }
            }

        }


        // Support methods.

        private void ChangeSelection(UnitController unit) {
            mainSelected = unit;
            mainSelection.Selected = mainSelected;
            unitSelectedEvent.Invoke(mainSelected);
        }

        private void SetTarget(UnitController unit) {
            unitTargetedEvent.Invoke(unit);
        }

        private void SetTarget(Vector3 position) {
            positionTargetedEvent.Invoke(position);
        }


    }

}

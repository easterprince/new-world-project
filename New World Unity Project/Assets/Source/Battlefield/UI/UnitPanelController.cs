using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NewWorld.Battlefield.Units;

namespace NewWorld.Battlefield.UI {

    public class UnitPanelController : MonoBehaviour {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649
        [SerializeField]
        private Text unitDescriptionText;
#pragma warning restore IDE0044, CS0414, CS0649

        UnitController selectedUnit;


        // Life cycle.

        private void Awake() {
            if (unitDescriptionText == null) {
                throw new MissingReferenceException($"Missing {nameof(unitDescriptionText)}.");
            }
        }

        private void Start() {
            PointerInterceptorController.EnsureInstance(this);
            PointerInterceptorController.Instance.ClickEvent.AddListener(ProcessClick);
            BattlefieldCameraController.EnsureInstance(this);
        }

        private void Update() {
            if (selectedUnit == null) {
                unitDescriptionText.text = "Click on unit to get its description.";
                return;
            }
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Selected unit: {selectedUnit.name}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Position: {selectedUnit.Position}");
            stringBuilder.AppendLine($"Rotation: {selectedUnit.Rotation}");
            unitDescriptionText.text = stringBuilder.ToString();
        }

        private void OnDestroy() {
            PointerInterceptorController.Instance?.ClickEvent.RemoveListener(ProcessClick);
        }


        // Triggered.

        private void ProcessClick(PointerEventData pointerEventData) {
            var pointerPosition = pointerEventData.position;
            var pointerRay = BattlefieldCameraController.Instance.CameraComponent.ScreenPointToRay(pointerPosition);
            var layerMask = LayerMask.GetMask("Units");
            Physics.Raycast(pointerRay, out RaycastHit raycastHit, float.PositiveInfinity, layerMask);
            var colliderHit = raycastHit.collider;
            if (colliderHit != null) {
                selectedUnit = colliderHit.transform.gameObject.GetComponent<UnitController>();
            }
        }


    }

}

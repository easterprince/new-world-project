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
        private Text unitNameText;
        [SerializeField]
        private Text unitDescriptionText;
#pragma warning restore IDE0044, CS0414, CS0649

        UnitController selectedUnit;


        // Life cycle.

        private void Start() {
            if (unitDescriptionText == null) {
                throw new MissingReferenceException($"Missing {nameof(unitDescriptionText)}.");
            }
            if (unitNameText == null) {
                throw new MissingReferenceException($"Missing {nameof(unitNameText)}.");
            }
            PointerInterceptorController.EnsureInstance(this);
            PointerInterceptorController.Instance.ClickEvent.AddListener(ProcessClick);
            BattlefieldCameraController.EnsureInstance(this);
        }

        private void Update() {
            if (selectedUnit == null) {
                unitNameText.text = "";
                unitDescriptionText.text = "Click on unit to get its description.";
                return;
            }
            unitNameText.text = selectedUnit.name;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Current condition: {selectedUnit.CurrentCondition?.ToString() ?? "Idle"}");
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
            } else {
                selectedUnit = null;
            }
        }


    }

}

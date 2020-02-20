using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NewWorld.Battlefield.UI {

    public class UnitPanelController : MonoBehaviour {

        // Static.

        private const string defaultText = "Click on unit to get its description.";


        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649
        [SerializeField]
        private Text unitDescriptionText;
#pragma warning restore IDE0044, CS0414, CS0649


        // Life cycle.

        private void Awake() {
            if (unitDescriptionText == null) {
                throw new MissingReferenceException($"Missing {nameof(unitDescriptionText)}.");
            }
        }

        private void Start() {
            PointerInterceptorController.EnsureInstance(this);
            PointerInterceptorController.Instance.ClickEvent.AddListener(ProcessClick);
            unitDescriptionText.text = defaultText;
        }

        private void OnDestroy() {
            PointerInterceptorController.Instance.ClickEvent.RemoveListener(ProcessClick);
        }


        // Triggered.

        private void ProcessClick(PointerEventData pointerEventData) {
            unitDescriptionText.text = pointerEventData.ToString();
        }


    }

}

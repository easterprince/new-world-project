using NewWorld.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battlefield.UI.Bars {

    public class VerticalVesselController : VesselController {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private GameObject fill;

#pragma warning restore IDE0044, CS0414, CS0649

        private RectTransform rectTransform;
        private RectTransform fillTransform;
        private Image fillImage;


        // Life cycle.

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
            GameObjects.ValidateComponent(rectTransform);
            GameObjects.ValidateReference(fill, "Fill");
            fillTransform = fill.GetComponent<RectTransform>();
            GameObjects.ValidateComponent(fillTransform, "Fill");
            fillImage = fill.GetComponent<Image>();
            GameObjects.ValidateComponent(fillImage, "Fill");
        }


        // Implemented.

        override protected void OnColorUpdate() {
            fillImage.color = Color;
        }

        override protected void OnFilledUpdate() {
            float height = rectTransform.rect.height;
            Vector2 offset = new Vector2(fillTransform.offsetMax.x, (Filled - 1) * height);
            fillTransform.offsetMax = offset;
        }


    }

}

using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battlefield.UI.Bars {

    public class VerticalBarController : BarController {

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
            if (rectTransform == null) {
                throw new MissingComponentException($"Missing {typeof(RectTransform)} component!");
            }
            if (fill == null) {
                throw new MissingReferenceException("Missing fill!");
            }
            fillTransform = fill.GetComponent<RectTransform>();
            if (fillTransform == null) {
                throw new MissingComponentException($"Fill is missing {typeof(RectTransform)} component!");
            }
            fillImage = fill.GetComponent<Image>();
            if (fillImage == null) {
                throw new MissingComponentException($"Fill is missing  {typeof(Image)} component!");
            }
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

using NewWorld.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Controllers.Battle.UI.Bars {
    
    public class VerticalBarController : BarController {

        // Fields.

        // Steady references.
        [SerializeField]
        private GameObject fill;

        // Components.
        private RectTransform rectTransform;
        private RectTransform fillTransform;
        private Image fillImage;


        // Life cycle.

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
            GameObjects.ValidateComponent(rectTransform);
            GameObjects.ValidateReference(fill, nameof(fill));
            fillTransform = fill.GetComponent<RectTransform>();
            GameObjects.ValidateComponent(fillTransform, nameof(fill));
            fillImage = fill.GetComponent<Image>();
            GameObjects.ValidateComponent(fillImage, nameof(fill));
            Color = fillImage.color;
            Filled = fillTransform.offsetMax.y / rectTransform.rect.height + 1;
        }


        // Implemented.

        private protected override void OnColorUpdate() {
            fillImage.color = Color;
        }

        private protected override void OnFilledUpdate() {
            float height = rectTransform.rect.height;
            Vector2 offset = new Vector2(fillTransform.offsetMax.x, (Filled - 1) * height);
            fillTransform.offsetMax = offset;
        }


    }

}

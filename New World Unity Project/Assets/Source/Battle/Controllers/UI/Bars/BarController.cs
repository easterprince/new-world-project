using UnityEngine;

namespace NewWorld.Battle.Controllers.UI.Bars {

    public abstract class BarController : MonoBehaviour {

        // Fields.

        private float filled = 0;
        private Color color = Color.black;


        // Properties.

        public float Filled {
            get => filled;
            set {
                filled = Mathf.Clamp01(value);
                OnFilledUpdate();
            }
        }

        public Color Color {
            get => color;
            set {
                color = value;
                OnColorUpdate();
            }
        }


        // Methods.

        private protected abstract void OnFilledUpdate();

        private protected abstract void OnColorUpdate();


    }

}

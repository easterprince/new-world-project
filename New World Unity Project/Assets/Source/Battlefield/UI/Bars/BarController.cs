using UnityEngine;

namespace NewWorld.Battlefield.UI.Bars {
    
    public abstract class BarController : MonoBehaviour {

        // Fields.

        private float filled;
        private Color color;


        // Properties.

        public float Filled {
            get => filled;
            set {
                filled = Mathf.Clamp(value, 0, 1);
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

        protected abstract void OnFilledUpdate();

        protected abstract void OnColorUpdate();


    }

}

using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battlefield.UI.Bars {

    public abstract class VesselController : MonoBehaviour {

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

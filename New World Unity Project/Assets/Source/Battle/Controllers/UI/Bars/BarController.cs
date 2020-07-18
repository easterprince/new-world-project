using UnityEngine;

namespace NewWorld.Battle.Controllers.UI.Bars {

    public abstract class BarController : MonoBehaviour {

        // Fields.

        private float filled = 0;


        // Properties.

        public float Filled {
            get => filled;
            set {
                filled = Mathf.Clamp01(filled);

            }
        }


        // Methods.

        private protected abstract void RedrawFilledLevel();


    }

}

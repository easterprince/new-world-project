using UnityEngine;
using UnityEngine.UI;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.UI.Bars {
    
    public class BarController : MonoBehaviour {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Text valueText;
        [SerializeField]
        private VesselController vessel;

#pragma warning restore IDE0044, CS0414, CS0649


        // Life cycle.

        private void Start() {
            GameObjects.ValidateReference(nameText, "Name Text");
            GameObjects.ValidateReference(valueText, "Value Text");
            GameObjects.ValidateReference(vessel, "Vessel");
        }


        // Methods.

        public string TypeText {
            get => nameText.text;
            set => nameText.text = value;
        }

        public string ValueText {
            get => valueText.text;
            set => valueText.text = value;
        }

        public float Filled {
            get => vessel.Filled;
            set => vessel.Filled = value;
        }

        public Color Color {
            get => vessel.Color;
            set => vessel.Color = value;
        }


    }

}

using NewWorld.Controllers.Battle.UI.Bars;
using NewWorld.Cores.Battle.Unit.Durability;
using NewWorld.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Controllers.Battle.UI.UnitPanel {

    public class DurabilitySectionController : MonoBehaviour {

        // Fields.

        // Steady references.
        [SerializeField]
        private BarController mainBar;
        [SerializeField]
        private Text durabilityKind;
        [SerializeField]
        private Text durabilityValue;


        // Life cycle.

        private void Start() {
            GameObjects.ValidateReference(mainBar, nameof(mainBar));
            GameObjects.ValidateReference(durabilityKind, nameof(durabilityKind));
            GameObjects.ValidateReference(durabilityValue, nameof(durabilityValue));
        }


        // Methods.

        public void SetDurability(DurabilityPresentation durabilityModule) {
            if (durabilityModule != null) {
                var durability = durabilityModule.Durability;
                var durabilityLimit = durabilityModule.DurabilityLimit;
                mainBar.Filled = durability / durabilityLimit;
                mainBar.Color = Color.red;
                durabilityValue.text = $"{durability}/{durabilityLimit}";
            } else {
                mainBar.Filled = 0;
                durabilityValue.text = "";
            }
        }


    }

}

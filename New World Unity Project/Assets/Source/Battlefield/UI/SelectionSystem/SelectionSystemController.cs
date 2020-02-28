using UnityEngine;
using NewWorld.Battlefield.Units;
using NewWorld.Utilities.Singletons;

namespace NewWorld.Battlefield.UI.SelectionSystem {
    
    public class SelectionSystemController : SceneSingleton<SelectionSystemController> {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649
        [SerializeField]
        private SelectionController mainSelection;
#pragma warning restore IDE0044, CS0414, CS0649


        // Life cycle.

        private void Start() {
            if (mainSelection == null) {
                throw new MissingReferenceException("Missing main selection controller!");
            }
        }


        // Methods.

        public void ChangeMainSelection(UnitController unit) {
            mainSelection.Selected = unit;
        }


    }

}

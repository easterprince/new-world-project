using NewWorld.Battle.Controllers.Unit;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using UnityEngine;

namespace NewWorld.Battle.Controllers.UI.Selection {

    public class SelectionSystemController : SteadyController {

        // Fields.

        private UnitController mainSelected = null;

        // Steady references.
        [SerializeField]
        private SelectionController mainSelection;


        // Properties.

        public UnitController MainSelected {
            get => mainSelected;
            set {
                ValidateBeingStarted();
                mainSelected = value;
                mainSelection.Selected = value;
            }
        }

        public SelectionController MainSelection {
            get => mainSelection;
            set {
                ValidateBeingNotFixed();
                mainSelection = value;
            }
        }



        // Life cycle.

        private protected override void OnStart() {
            GameObjects.ValidateReference(mainSelection, nameof(mainSelection));
        }


    }

}

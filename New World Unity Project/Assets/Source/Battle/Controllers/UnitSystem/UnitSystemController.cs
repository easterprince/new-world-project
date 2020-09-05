using NewWorld.Battle.Controllers.Unit;
using NewWorld.Battle.Cores.Unit;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Controllers.UnitSystem {

    public class UnitSystemController : BuildableController {

        // Fields.

        // Structure.
        private UnitSystemPresentation presentation = null;
        private ClassState<UnitPresentation>.StateWrapper currentState = null;
        private readonly Dictionary<UnitPresentation, GameObject> presentationsToUnits = new Dictionary<UnitPresentation, GameObject>();

        // Steady references.
        [SerializeField]
        private GameObject unitsObject;


        // Properties.

        public UnitSystemPresentation Presentation => presentation;

        public GameObject UnitsObject {
            get => unitsObject;
            set {
                ValidateBeingNotStarted();
                unitsObject = value;
            }
        }


        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();
            GameObjects.ValidateReference(unitsObject, nameof(unitsObject));
        }

        private void LateUpdate() {
            while (currentState != null && !currentState.IsLatest) {
                currentState = currentState.Transit(out UnitPresentation unitPresentation);
                if (presentation.HasUnit(unitPresentation)) {
                    CreateUnit(unitPresentation);
                } else {
                    DestroyUnit(unitPresentation);
                }
            }
        }


        // Building.

        public void Build(UnitSystemPresentation presentation) {
            if (presentation == null) {
                throw new ArgumentNullException(nameof(presentation));
            }
            ValidateBeingStarted();
            ValidateNotStartedBuilding();

            // Set fields.
            SetStartedBuilding();
            this.presentation = presentation;
            currentState = presentation.State;

            // Create unit objects.
            foreach (var unitPresentation in presentation) {
                CreateUnit(unitPresentation);
            }

            SetFinishedBuilding();

        }


        // Support methods.

        private void CreateUnit(UnitPresentation unitPresentation) {
            if (presentationsToUnits.ContainsKey(unitPresentation)) {
                return;
            }
            GameObject unit = UnitController.BuildUnit(unitPresentation).gameObject;
            unit.transform.parent = unitsObject.transform;
            GameObjects.SetLayerRecursively(unit, unitsObject.layer);
            presentationsToUnits[unitPresentation] = unit;
        }

        private void DestroyUnit(UnitPresentation unitPresentation) {
            if (!presentationsToUnits.TryGetValue(unitPresentation, out GameObject unit)) {
                return;
            }
            presentationsToUnits.Remove(unitPresentation);
            if (unit != null) {
                Destroy(unit);
            }
        }


    }

}

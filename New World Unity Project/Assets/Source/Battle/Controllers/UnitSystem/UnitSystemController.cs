using NewWorld.Battle.Controllers.Unit;
using NewWorld.Battle.Cores.Unit;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using NewWorld.Utilities.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Controllers.UnitSystem {

    public class UnitSystemController : BuildableController {

        // Fields.

        // Structure.
        private UnitSystemPresentation presentation = null;
        private readonly Dictionary<UnitPresentation, GameObject> presentationsToUnits = new Dictionary<UnitPresentation, GameObject>();
        private readonly ActionQueue actionQueue = new ActionQueue();

        // Steady references.
        [SerializeField]
        private GameObject unitsObject;


        // Properties.

        public UnitSystemPresentation Presentation => presentation;

        public GameObject UnitsObject {
            get => unitsObject;
            set {
                ValidateBeingNotFixed();
                unitsObject = value;
            }
        }


        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();
            GameObjects.ValidateReference(unitsObject, nameof(unitsObject));
        }

        private void LateUpdate() {
            actionQueue.RunAll();
        }

        private protected override void OnDestroy() {
            if (presentation != null) {
                presentation.AdditionEvent.RemoveSubscriber(actionQueue);
                presentation.RemovalEvent.RemoveSubscriber(actionQueue);
            }
            base.OnDestroy();
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

            // Create unit objects.
            foreach (var unitPresentation in presentation) {
                CreateUnit(unitPresentation);
            }
            presentation.AdditionEvent.AddAction(actionQueue, CreateUnit);
            presentation.RemovalEvent.AddAction(actionQueue, DestroyUnit);

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

﻿using NewWorld.Battle.Controllers.Unit;
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

        // Unity references.
        [SerializeField]
        private GameObject unitsObject;


        // Properties.

        public UnitSystemPresentation Presentation {
            get => presentation;
            set {
                if (presentation == value) {
                    return;
                }
                if (presentation != null) {
                    presentation.AdditionEvent.RemoveSubscriber(actionQueue);
                }
                DestroyUnits();
                presentation = value;
                CreateUnits();
                if (presentation != null) {
                    presentation.AdditionEvent.AddAction(actionQueue, CreateUnit);
                    presentation.AdditionEvent.AddAction(actionQueue, DestroyUnit);
                }
                Built = (presentation != null);
            }
        }

        public GameObject UnitsObject {
            get => unitsObject;
            set {
                DestroyUnits();
                unitsObject = value;
                CreateUnits();
            }
        }


        // Life cycle.

        private void LateUpdate() {
            actionQueue.RunAll();
        }


        // Support methods.

        private void CreateUnits() {
            if (presentation == null || unitsObject == null) {
                return;
            }
            foreach (var unitPresentation in presentation) {
                CreateUnit(unitPresentation);
            }
        }

        private void DestroyUnits() {
            var unitPresentations = new List<UnitPresentation>(presentationsToUnits.Keys);
            foreach (var unit in unitPresentations) {
                DestroyUnit(unit);
            }
        }

        private void CreateUnit(UnitPresentation unitPresentation) {
            if (unitsObject == null || presentationsToUnits.ContainsKey(unitPresentation)) {
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

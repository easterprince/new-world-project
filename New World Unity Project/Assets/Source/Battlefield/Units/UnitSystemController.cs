using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Composition;
using NewWorld.Battlefield.Loading;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Units {

    public class UnitSystemController : SceneSingleton<UnitSystemController> {

        // Fields.

        private int currentVisionDirection;
        private List<UnitController> units;


        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
            currentVisionDirection = 0;
            units = new List<UnitController>();
            int index = 0;
            foreach (UnitDescription unitDescription in BattlefieldLoader.Instance.Units) {
                UnitController unit = UnitController.BuildUnit(unitDescription, currentVisionDirection, $"Unit {index++}");
                unit.transform.parent = transform;
                units.Add(unit);
            }
        }


        // External control.

        public void Rotate(int visionDirection) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            currentVisionDirection = visionDirection;
            foreach (UnitController unit in units) {
                unit.Rotate(currentVisionDirection);
            }
        }

    }

}

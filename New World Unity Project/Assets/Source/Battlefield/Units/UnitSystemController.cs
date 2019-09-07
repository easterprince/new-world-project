using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Composition;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController : SceneSingleton<UnitSystemController> {

        // Fields.

        private int currentVisionDirection;
        private HashSet<UnitController> units;


        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
            InitializeIntentionProcessing();
            currentVisionDirection = 0;
            units = new HashSet<UnitController>();
        }

        private void Update() {
            ProcessIntentions();
        } 


        // External control.

        public void Load(List<UnitDescription> unitDescriptions) {
            int index = 0;
            foreach (UnitDescription unitDescription in unitDescriptions) {
                UnitController unit = UnitController.BuildUnit(unitDescription, currentVisionDirection, $"Unit {index++}");
                unit.transform.parent = transform;
                units.Add(unit);
            }
        }

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

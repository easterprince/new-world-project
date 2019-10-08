using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController : SceneSingleton<UnitSystemController> {

        // Fields.

        private HashSet<UnitController> units;


        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
            InitializeIntentionProcessing();
            units = new HashSet<UnitController>();
        }

        private void Update() {
            ProcessIntentions();
        } 


        // External control.

        public void Load(List<UnitDescription> unitDescriptions) {
            int index = 0;
            foreach (UnitDescription unitDescription in unitDescriptions) {
                UnitController unit = UnitController.BuildUnit(unitDescription, $"Unit {index++}");
                unit.transform.parent = transform;
                units.Add(unit);
            }
        }

    }

}

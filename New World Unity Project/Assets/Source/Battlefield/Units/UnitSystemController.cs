using UnityEngine;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Units {

    class UnitSystemController : SceneSingleton<UnitSystemController> {

        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
        }

    }

}

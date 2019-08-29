using UnityEngine;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Units {

    public class UnitSystemController : SceneSingleton<UnitSystemController> {

        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
        }

    }

}

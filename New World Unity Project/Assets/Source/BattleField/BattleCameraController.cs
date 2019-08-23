using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NewWorld.Utilities;
using NewWorld.Utilities.Singletones;

namespace NewWorld.BattleField {

    public class BattleCameraController : SceneSingleton<BattleCameraController> {

        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
        }


        // TODO: Implement motion.
        // TODO: Implement rotation.

    }

}

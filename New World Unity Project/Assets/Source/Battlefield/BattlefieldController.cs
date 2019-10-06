using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Composition;

namespace NewWorld.Battlefield {

    public class BattlefieldController : SceneSingleton<BattlefieldController> {

        // Variables.

        private bool battleStarted;


        // Properties.

        public bool BattleStarted => battleStarted;


        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
            battleStarted = false;
        }

        private void Update() {}


        // Controlling methods.

        public void StartBattle() {
            battleStarted = true;
            BattlefieldCameraController.Instance.Place(Vector3.zero);
        }


    }

}

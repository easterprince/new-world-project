using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units;

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


        // Loading.

        public IEnumerator Load(BattlefieldDescription description) {
            if (description == null) {
                throw new System.ArgumentNullException(nameof(description));
            }
            yield return StartCoroutine(MapController.Instance.Load(description.MapDescription));
            yield return StartCoroutine(UnitSystemController.Instance.Load(description.UnitDescriptions));
        }

        public void StartBattle() {
            battleStarted = true;
        }


    }

}

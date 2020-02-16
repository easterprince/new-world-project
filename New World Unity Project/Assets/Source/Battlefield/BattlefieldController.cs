using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletons;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units;
using UnityEngine.Events;

namespace NewWorld.Battlefield {

    public class BattlefieldController : LoadableSingleton<BattlefieldController, BattlefieldDescription> {

        // Variables.

        private bool battleStarted;


        // Properties.

        public bool BattleStarted => battleStarted;


        // Life cycle.

        override private protected void Awake() {
            base.Awake();
            Instance = this;
            battleStarted = false;
        }


        // Loading

        override protected IEnumerator OnReload(BattlefieldDescription description) {
            if (description == null) {
                throw new System.ArgumentNullException(nameof(description));
            }
            MapController.Instance.StartReloading(description.MapDescription);
            while (!MapController.Instance.Loaded) {
                yield return null;
            }
            UnitSystemController.Instance.StartReloading(description.UnitDescriptions);
            while (!UnitSystemController.Instance.Loaded) {
                yield return null;
            }
        }

        public void StartBattle() {
            battleStarted = true;
        }


    }

}

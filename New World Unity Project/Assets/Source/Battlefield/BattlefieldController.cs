using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletons;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units;
using UnityEngine.Events;

namespace NewWorld.Battlefield {

    public class BattlefieldController : ReloadableSingleton<BattlefieldController, BattlefieldDescription> {

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

        private void Start() {
            Time.timeScale = 0;
        }


        // Loading

        override public void StartReloading(BattlefieldDescription description) {
            if (description == null) {
                throw new System.ArgumentNullException(nameof(description));
            }

            // TODO. Make it possible to abort current reloading and begin new one.
            if (!Loaded) {
                throw new System.NotImplementedException();
            }
            Loaded = false;

            void afterUnitSystemLoaded() {
                UnitSystemController.Instance.LoadedEvent.RemoveListener(afterUnitSystemLoaded);
                Loaded = true;
            }

            void afterMapLoaded() {
                MapController.Instance.LoadedEvent.RemoveListener(afterMapLoaded);
                UnitSystemController.Instance.LoadedEvent.AddListener(afterUnitSystemLoaded);
                UnitSystemController.Instance.StartReloading(description.UnitDescriptions);
            }

            MapController.Instance.LoadedEvent.AddListener(afterMapLoaded);
            MapController.Instance.StartReloading(description.MapDescription);

        }

        public void StartBattle() {
            Time.timeScale = 1;
            battleStarted = true;
        }


    }

}

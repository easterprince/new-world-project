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
        private BattlefieldDescription loadedDescription;


        // Properties.

        public bool BattleStarted => battleStarted;


        // Life cycle.

        override private protected void Awake() {
            base.Awake();
            battleStarted = false;
        }

        private void Start() {
            Time.timeScale = 0;
        }

        override private protected void OnDestroy() {
            MapController.Instance?.LoadedEvent.RemoveListener(AfterMapLoaded);
            UnitSystemController.Instance?.LoadedEvent.RemoveListener(AfterUnitSystemLoaded);
            base.OnDestroy();
        }


        // Loading.

        override public void StartReloading(BattlefieldDescription description) {
            if (description == null) {
                throw new System.ArgumentNullException(nameof(description));
            }

            // TODO. Make it possible to abort current reloading and begin new one.
            if (!Loaded) {
                throw new System.NotImplementedException();
            }
            Loaded = false;
            loadedDescription = description;

            MapController.Instance.LoadedEvent.AddListener(AfterMapLoaded);
            MapController.Instance.StartReloading(description.MapDescription);

        }


        // Public methods.

        public void StartBattle() {
            Time.timeScale = 1;
            battleStarted = true;
        }


        // Event handlers.

        private void AfterMapLoaded() {
            MapController.Instance.LoadedEvent.RemoveListener(AfterMapLoaded);
            UnitSystemController.Instance.LoadedEvent.AddListener(AfterUnitSystemLoaded);
            UnitSystemController.Instance.StartReloading(loadedDescription.UnitDescriptions);
        }

        private void AfterUnitSystemLoaded() {
            UnitSystemController.Instance.LoadedEvent.RemoveListener(AfterUnitSystemLoaded);
            Loaded = true;
        }


    }

}

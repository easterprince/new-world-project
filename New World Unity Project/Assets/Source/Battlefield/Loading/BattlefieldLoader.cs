using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Map.Generation;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Loading {

    public class BattlefieldLoader : SceneSingleton<BattlefieldLoader> {

        // Fields.

        private bool loaded;

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private BattlefieldLoadingScreenController loadingScreen;

#pragma warning restore IDE0044, CS0414, CS0649

        private MapDescription mapDescription;
        private Task<MapDescription> mapDescriptionLoading;

        // Properties.

        public MapDescription MapDescription => mapDescription;


        // Life cycle.

        override protected void Awake() {
            base.Awake();
            Instance = this;
            loaded = false;
        }

        private void Start() {
            mapDescriptionLoading = Task.Run(LoadMapDescription);
        }

        private void Update() {
            if (!loaded) {
                if (mapDescriptionLoading.IsCompleted) {
                    mapDescription = mapDescriptionLoading.Result;
                    BattlefieldController.Instance.LoadBattle();
                    loadingScreen.LoadingAnimation = false;
                    loaded = true;
                }
            }
            if (loaded) {
                if (Input.anyKey) {
                    loadingScreen.gameObject.SetActive(false);
                    BattlefieldController.Instance.StartBattle();
                    Destroy(this.gameObject);
                }
            }
        }


        // Map loading.

        private MapDescription LoadMapDescription() {
            ExperimentalMapGenerator mapGenerator = new ExperimentalMapGenerator {
                Seed = 123,
                Size = new Vector2Int(60, 60),
                HeightLimit = 5
            };
            return mapGenerator.Generate();
        }

    }

}
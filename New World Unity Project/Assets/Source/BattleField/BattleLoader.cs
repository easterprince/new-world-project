using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using NewWorld.Utilities;
using NewWorld.BattleField.Map;
using NewWorld.Utilities.Singletones;

namespace NewWorld.BattleField {

    public class BattleLoader : SceneSingleton<BattleLoader> {

        // Fields.

#pragma warning disable IDE0044, CS0414

        [SerializeField]
        private LoadingScreenController loadingScreen = null;

        [SerializeField]
        private BattleCameraController battleCamera = null;

        [SerializeField]
        private MapController map = null;

#pragma warning restore IDE0044, CS0414

        private MapDescription mapDescription;
        private Task<MapDescription> mapDescriptionLoading;

        // Properties.

        public MapDescription MapDescription => mapDescription;


        // Life cycle.

        override protected void Awake() {
            base.Awake();
            Instance = this;
        }

        private void Start() {
            mapDescriptionLoading = Task.Run(LoadMapDescription);
        }

        private void Update() {
            if (loadingScreen.LoadingAnimation) {
                if (mapDescriptionLoading.IsCompleted) {
                    mapDescription = mapDescriptionLoading.Result;
                    loadingScreen.LoadingAnimation = false;
                }
            } else {
                if (Input.anyKey) {
                    FinishLoading();
                }
            }
        }


        // Support.

        private void FinishLoading() {
            if (!mapDescriptionLoading.IsCompleted) {
                return;
            }
            loadingScreen.gameObject.SetActive(false);
            map.gameObject.SetActive(true);
        }


        // Map loading.

        private MapDescription LoadMapDescription() {
            System.Random random = new System.Random(123);
            int size = 10;
            MapDescription mapDescription = new MapDescription(new Vector2Int(size, size), 2f);
            for (int i = 0; i < 100000; ++i) {
                Vector2Int position = new Vector2Int(random.Next(0, mapDescription.Size.x), random.Next(0, mapDescription.Size.y));
                mapDescription.SetSurfaceNode(position, new NodeDescription(3 * (float) random.NextDouble()));
            }
            return mapDescription;
        }

    }

}
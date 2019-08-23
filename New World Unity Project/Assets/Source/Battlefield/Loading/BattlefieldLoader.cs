using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using NewWorld.Utilities;
using NewWorld.Battlefield.Map;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Loading {

    public class BattlefieldLoader : SceneSingleton<BattlefieldLoader> {

        // Fields.

        private bool loaded;

#pragma warning disable IDE0044, CS0414

        [SerializeField]
        private BattlefieldLoadingScreenController loadingScreen = null;

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
            loaded = false;
        }

        private void Start() {
            mapDescriptionLoading = Task.Run(LoadMapDescription);
        }

        private void Update() {
            if (!loaded) {
                if (mapDescriptionLoading.IsCompleted) {
                    mapDescription = mapDescriptionLoading.Result;
                    loadingScreen.LoadingAnimation = false;
                    loaded = true;
                }
            }
            if (loaded) {
                if (Input.anyKey) {
                    loadingScreen.gameObject.SetActive(false);
                    map.gameObject.SetActive(true);
                    BattlefieldCameraController.Instance.Place(Vector3.zero);
                    Destroy(this.gameObject);
                }
            }
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
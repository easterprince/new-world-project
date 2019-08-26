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
            int size = 60;
            MapDescription mapDescription = new MapDescription(new Vector2Int(size, size), 10f);
            for (int i = 0; i < 1000; ++i) {
                Vector2Int position = new Vector2Int(random.Next(0, mapDescription.Size.x), random.Next(0, mapDescription.Size.y));
                mapDescription.SetSurfaceNode(position, new NodeDescription(8 * (float) random.NextDouble()));
            }
            for (int i = 0; i < 20000; ++i) {
                Vector2Int position = new Vector2Int(random.Next(0, mapDescription.Size.x), random.Next(0, mapDescription.Size.y));
                float height = 0;
                for (int dx = -1; dx <= 1; ++dx) {
                    for (int dy = -1; dy <= 1; ++dy) {
                        height += mapDescription.GetSurfaceNode(new Vector2Int(position.x + dx, position.y + dy))?.Height ?? 0;
                    }
                }
                height /= 9;
                mapDescription.SetSurfaceNode(position, new NodeDescription(height));
            }
            return mapDescription;
        }

    }

}
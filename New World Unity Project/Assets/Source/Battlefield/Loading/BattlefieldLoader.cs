using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Map.Generation;
using NewWorld.Battlefield.Units;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Loading {

    public class BattlefieldLoader : MonoBehaviour {

        // Fields.

        private bool loaded;

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private BattlefieldLoadingScreenController loadingScreen;

#pragma warning restore IDE0044, CS0414, CS0649

        private MapDescription mapDescription;
        private List<UnitDescription> unitDescriptions;
        private Task battlefieldLoading;


        // Life cycle.

        private void Awake() {
            loaded = false;
        }

        private void Start() {
            battlefieldLoading = Task.Run(LoadBattlefield);
        }

        private void Update() {
            if (!loaded) {
                if (battlefieldLoading.IsCompleted) {
                    loaded = true;
                    MapController.Instance.Load(mapDescription);
                    UnitSystemController.Instance.Load(unitDescriptions);
                    loadingScreen.LoadingAnimation = false;
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


        // Battlefield loading.

        private void LoadBattlefield() {
            int seed = 123;
            System.Random random = new System.Random(seed);
            ExperimentalMapGenerator mapGenerator = new ExperimentalMapGenerator {
                Seed = seed,
                Size = new Vector2Int(80, 120),
                HeightLimit = 5
            };
            mapDescription = mapGenerator.Generate();
            unitDescriptions = new List<UnitDescription>();
            int unitsCount = 300;
            for (int i = 0; i < unitsCount; ++i) {
                Vector2Int position;
                do {
                    position = new Vector2Int(random.Next(mapDescription.Size.x), random.Next(mapDescription.Size.y));
                } while (mapDescription.GetSurfaceNode(position) == null);
                unitDescriptions.Add(new UnitDescription(position, 0.48f));
            }
        }

    }

}
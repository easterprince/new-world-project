using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Map.Generation;
using NewWorld.Battlefield.Units;

namespace NewWorld.Battlefield.Loading {

    public class BattlefieldLoader : MonoBehaviour {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private BattlefieldLoadingScreenController loadingScreen;

#pragma warning restore IDE0044, CS0414, CS0649

        private bool readyToLoad;
        private bool readyToStart;

        private BattlefieldDescription battlefieldDescription;


        // Life cycle.

        private void Awake() {
            readyToLoad = false;
            readyToStart = false;
        }

        private void Start() {
            StartCoroutine(PlanBattlefield());
        }

        private void Update() {
            if (readyToLoad) {
                StartCoroutine(LoadBattlefield());
                readyToLoad = false;
            }
            if (readyToStart) {
                if (loadingScreen.LoadingAnimation) {
                    loadingScreen.LoadingAnimation = false;
                }
                if (Input.anyKey) {
                    readyToStart = false;
                    loadingScreen.gameObject.SetActive(false);
                    BattlefieldController.Instance.StartBattle();
                    Destroy(this.gameObject);
                }
            }
        }


        // Battlefield planning and loading.

        private IEnumerator PlanBattlefield() {
            Task battlefieldPlanning = Task.Run(CreateBattlefield);
            while (!battlefieldPlanning.IsCompleted) {
                yield return null;
            }
            readyToLoad = true;
        }

        private void CreateBattlefield() {
            int seed = 123;
            System.Random random = new System.Random(seed);
            ExperimentalMapGenerator mapGenerator = new ExperimentalMapGenerator {
                Size = new Vector2Int(80, 120),
                HeightLimit = 5
            };
            MapDescription mapDescription = mapGenerator.Generate(seed);
            List<UnitDescription> unitDescriptions = new List<UnitDescription>();
            int unitsCount = 300;
            for (int i = 0; i < unitsCount; ++i) {
                Vector2Int position;
                do {
                    position = new Vector2Int(random.Next(mapDescription.Size.x), random.Next(mapDescription.Size.y));
                } while (mapDescription.GetSurfaceNode(position) == null);
                unitDescriptions.Add(new UnitDescription(position, 0.48f));
            }
            battlefieldDescription = new BattlefieldDescription(mapDescription, unitDescriptions);
        }

        private IEnumerator LoadBattlefield() {
            yield return StartCoroutine(BattlefieldController.Instance.Load(battlefieldDescription));
            readyToStart = true;
        }


    }

}
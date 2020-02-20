using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Loading.Generation;
using NewWorld.Battlefield.Units;

namespace NewWorld.Battlefield.Loading {

    public class BattlefieldLoader : MonoBehaviour {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private BattlefieldLoadingScreenController loadingScreen;

#pragma warning restore IDE0044, CS0414, CS0649

        private bool readyToLoad = false;
        private bool readyToStart = false;

        private BattlefieldDescription battlefieldDescription;


        // Life cycle.

        private void Start() {
            StartCoroutine(PlanBattlefield());
        }

        private void Update() {
            if (readyToLoad) {
                LoadBattlefield();
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
            Task<BattlefieldDescription> battlefieldPlanning = Task.Run(CreateBattlefield);
            while (!battlefieldPlanning.IsCompleted) {
                yield return null;
            }
            battlefieldDescription = battlefieldPlanning.Result;
            readyToLoad = true;
        }

        private BattlefieldDescription CreateBattlefield() {
            int seed = 123;
            System.Random random = new System.Random(seed);
            ExperimentalMapGenerator mapGenerator = new ExperimentalMapGenerator {
                Size = new Vector2Int(80, 120),
                HeightLimit = 5
            };
            MapDescription mapDescription = mapGenerator.Generate(seed);
            List<UnitDescription> unitDescriptions = new List<UnitDescription>();
            int unitsCount = 600;
            for (int i = 0; i < unitsCount; ++i) {
                Vector2Int position;
                bool repeat;
                do {
                    position = new Vector2Int(random.Next(mapDescription.Size.x), random.Next(mapDescription.Size.y));
                    repeat = false;
                    foreach (UnitDescription description in unitDescriptions) {
                        if (description.ConnectedNode == position) {
                            repeat = true;
                            break;
                        }
                    }
                } while (repeat || mapDescription[position].Type == NodeDescription.NodeType.Abyss);
                unitDescriptions.Add(new UnitDescription(position, 0.48f));
            }
            return new BattlefieldDescription(mapDescription, unitDescriptions);
        }

        private void LoadBattlefield() {

            void AfterBattlefieldLoad() {
                BattlefieldController.Instance.LoadedEvent.RemoveListener(AfterBattlefieldLoad);
                readyToStart = true;
            }

            BattlefieldController.Instance.LoadedEvent.AddListener(AfterBattlefieldLoad);
            BattlefieldController.Instance.StartReloading(battlefieldDescription);
        }


    }

}
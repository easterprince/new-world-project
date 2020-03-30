using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Loading.Generation;
using NewWorld.Battlefield.Unit;
using NewWorld.Battlefield.UI.LoadingScreen;

namespace NewWorld.Battlefield.Loading {

    public class BattlefieldLoader : MonoBehaviour {

        // Fields.

        private bool readyToLoad = false;

        private BattlefieldDescription battlefieldDescription;


        // Life cycle.

        private void Start() {
            StartCoroutine(PlanBattlefield());
        }

        private void Update() {
            if (readyToLoad) {
                BattlefieldController.Instance.StartReloading(battlefieldDescription);
                readyToLoad = false;
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
            List<UnitTemplate> unitDescriptions = new List<UnitTemplate>();
            int unitsCount = 600;
            for (int i = 0; i < unitsCount; ++i) {
                Vector2Int position;
                bool repeat;
                do {
                    position = new Vector2Int(random.Next(mapDescription.Size.x), random.Next(mapDescription.Size.y));
                    repeat = false;
                    foreach (UnitTemplate description in unitDescriptions) {
                        if (description.ConnectedNode == position) {
                            repeat = true;
                            break;
                        }
                    }
                } while (repeat || mapDescription[position].Type == NodeDescription.NodeType.Abyss);
                var template = new UnitTemplate() {
                    ConnectedNode = position
                };
                unitDescriptions.Add(template);
            }
            return new BattlefieldDescription(mapDescription, unitDescriptions);
        }


    }

}
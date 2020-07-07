using NewWorld.Battle.Controllers.UnitSystem;
using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Generation.Map;
using NewWorld.Battle.Cores.Generation.Units;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Battlefield {

    public class BattlefieldController : MonoBehaviour {

        // Fields.

        private BattlefieldCore core;

        [SerializeField]
        private UnitSystemController unitSystem;


        // Properties.

        public UnitSystemController UnitSystem {
            get => unitSystem;
            set {
                unitSystem = value;
                if (unitSystem != null) {
                    unitSystem.Presentation = core.UnitSystem;
                }
            }
        }


        // Life cycle.

        private void Start() {

            var mapGenerator = new FullOfHolesMapGenerator() {
                HeightLimit = 10,
                Size = new Vector2Int(20, 20)
            };
            var mapCore = mapGenerator.Generate(0);

            var unitSystemGenerator = new UniformUnitSystemGenerator() {
                UnitCount = 10
            };
            var unitSystemCore = unitSystemGenerator.Generate(0, mapCore);

            core = new BattlefieldCore(mapCore, unitSystemCore);
            if (unitSystem != null) {
                unitSystem.Presentation = core.UnitSystem;
            }

        }

        private void Update() {
            core.Update();
        }


    }

}

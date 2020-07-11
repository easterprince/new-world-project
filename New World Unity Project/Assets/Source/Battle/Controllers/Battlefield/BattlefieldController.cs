using NewWorld.Battle.Controllers.Map;
using NewWorld.Battle.Controllers.UnitSystem;
using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Generation.Map;
using NewWorld.Battle.Cores.Generation.Units;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Battlefield {

    public class BattlefieldController : MonoBehaviour {

        // Fields.

        private BattlefieldCore core = null;
        private bool ready = false;

        // Controller references.
        [SerializeField]
        private MapController map;
        [SerializeField]
        private UnitSystemController unitSystem;


        // Properties.

        public BattlefieldPresentation Presentation => core.Presentation;

        public bool Ready => ready;

        public MapController Map {
            get => map;
            set {
                map = value;
                if (map != null) {
                    map.Presentation = core.Map;
                }
            }
        }

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

            // Generate core.
            if (core == null) {

                // Generate map.
                var mapGenerator = new FullOfHolesMapGenerator() {
                    HeightLimit = 10,
                    Size = new Vector2Int(40, 40)
                };
                var mapCore = mapGenerator.Generate(0);

                // Generate units.
                var unitSystemGenerator = new UniformUnitSystemGenerator() {
                    UnitCount = 10
                };
                var unitSystemCore = unitSystemGenerator.Generate(0, mapCore);

                // Assemble core.
                core = new BattlefieldCore(mapCore, unitSystemCore);

            }

            // Assign presentations to controllers.
            if (unitSystem != null) {
                unitSystem.Presentation = core.UnitSystem;
            }
            if (map != null) {
                map.Presentation = core.Map;
            }
            ready = true;

        }

        private void Update() {
            core?.Update();
        }


    }

}

using NewWorld.Battle.Controllers.Map;
using NewWorld.Battle.Controllers.UnitSystem;
using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Generation.Map;
using NewWorld.Battle.Cores.Generation.Units;
using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities.Controllers;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Battlefield {

    public class BattlefieldController : BuildableController {

        // Fields.

        private BattlefieldCore core = null;

        // Controller references.
        [SerializeField]
        private MapController map;
        [SerializeField]
        private UnitSystemController unitSystem;

        // Tasks.
        private CancellationTokenSource mapGenerationCancellation = null;
        private Task<MapCore> mapGenerationTask = null;
        private CancellationTokenSource unitSystemGenerationCancellation = null;
        private Task<UnitSystemCore> unitSystemGenerationTask = null;


        // Properties.

        public BattlefieldPresentation Presentation => core.Presentation;

        public MapController Map {
            get => map;
            set {
                map = value;
                if (map != null) {
                    map.Presentation = core?.Map;
                }
            }
        }

        public UnitSystemController UnitSystem {
            get => unitSystem;
            set {
                unitSystem = value;
                if (unitSystem != null) {
                    unitSystem.Presentation = core?.UnitSystem;
                }
            }
        }


        // Life cycle.

        private void Start() {

            // Start generating map.
            var mapGenerator = new FullOfHolesMapGenerator() {
                HeightLimit = 10,
                Size = new Vector2Int(100, 100)
            };
            mapGenerationCancellation = new CancellationTokenSource();
            mapGenerationTask = Task.Run(
                () => mapGenerator.Generate(0, mapGenerationCancellation.Token));

        }

        private void Update() {
            
            // Check core generation progress.
            if (core == null) {

                // Check map generation task.
                if (mapGenerationTask != null && unitSystemGenerationTask == null) {
                    if (mapGenerationTask.IsCompleted) {

                        var mapCore = mapGenerationTask.Result;

                        // Generate units.
                        var unitSystemGenerator = new UniformUnitSystemGenerator() {
                            Map = mapCore.Presentation,
                            UnitCount = 60
                        };
                        unitSystemGenerationCancellation = new CancellationTokenSource();
                        unitSystemGenerationTask = Task.Run(
                            () => unitSystemGenerator.Generate(0, unitSystemGenerationCancellation.Token));

                    }
                }

                // Check unit generation task.
                if (unitSystemGenerationTask != null) {
                    if (unitSystemGenerationTask.IsCompleted) {

                        var mapCore = mapGenerationTask.Result;
                        var unitSystemCore = unitSystemGenerationTask.Result;
                        CancelMapGeneration();
                        CancelUnitSystemGeneration();

                        // Assemble core.
                        core = new BattlefieldCore(mapCore, unitSystemCore);

                        // Assign presentations to controllers.
                        if (unitSystem != null) {
                            unitSystem.Presentation = core.UnitSystem;
                        }
                        if (map != null) {
                            map.Presentation = core.Map;
                            map.ExecuteWhenBuilt(this, () => Built = true);
                        }

                    }
                }

            }

            // Update core.
            core?.Update();

        }

        private protected override void OnDestroy() {
            CancelMapGeneration();
            CancelUnitSystemGeneration();
            base.OnDestroy();
        }


        // Support methods.

        private void CancelMapGeneration() {
            if (mapGenerationCancellation != null) {
                mapGenerationCancellation.Cancel();
                mapGenerationCancellation = null;
                mapGenerationTask = null;
            }
        }

        private void CancelUnitSystemGeneration() {
            if (unitSystemGenerationCancellation != null) {
                unitSystemGenerationCancellation.Cancel();
                unitSystemGenerationCancellation = null;
                unitSystemGenerationTask = null;
            }
        }


    }

}

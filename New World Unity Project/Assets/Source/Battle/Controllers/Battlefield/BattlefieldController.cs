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
        private string loadingStatus = "Waiting...";

        // Controller references.
        [SerializeField]
        private MapController map;
        [SerializeField]
        private UnitSystemController unitSystem;

        // Tasks.
        private CancellationTokenSource coreGenerationCancellation = null;
        private Task<BattlefieldCore> coreGenerationTask = null;


        // Properties.

        public BattlefieldPresentation Presentation => core?.Presentation;

        public string LoadingStatus => loadingStatus;

        public MapController Map {
            get => map;
            set => map = value;
        }

        public UnitSystemController UnitSystem {
            get => unitSystem;
            set => unitSystem = value;
        }


        // Life cycle.

        private void Start() {

            // Start generating map.
            StartCoreGeneration();

        }

        private void Update() {
            
            // Check core generation progress.
            if (core == null && coreGenerationTask.IsCompleted) {

                loadingStatus = "Drawing everything...";
                core = coreGenerationTask.Result;
                CancelCoreGeneration();

                if (map != null) {
                    map.StartBuilding(core.Map);
                    map.ExecuteWhenBuilt(this, UpdateWhenBuilt);
                }

                if (unitSystem != null) {
                    unitSystem.Presentation = core.UnitSystem;
                }

                UpdateWhenBuilt();

            }

            // Update core.
            if (Built) {
                core?.Update();
            }

        }

        private protected override void OnDestroy() {
            CancelCoreGeneration();
            base.OnDestroy();
        }


        // Core generation.

        private void StartCoreGeneration() {
            loadingStatus = "Generating game data...";
            CancelCoreGeneration();
            coreGenerationCancellation = new CancellationTokenSource();
            var cancellationToken = coreGenerationCancellation.Token;
            coreGenerationTask = Task.Run(() => GenerateCore(cancellationToken));
        }

        private static BattlefieldCore GenerateCore(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            // Generate map.
            var mapGenerator = new FullOfHolesMapGenerator() {
                HeightLimit = 10,
                Size = new Vector2Int(100, 100)
            };
            var map = mapGenerator.Generate(0, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            // Generate unit system.
            var unitSystemGenerator = new UniformUnitSystemGenerator() {
                Map = map.Presentation,
                UnitCount = 60
            };
            var unitSystem = unitSystemGenerator.Generate(0, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            // Assemble core.
            var core = new BattlefieldCore(map, unitSystem);
            return core;
        }

        private void CancelCoreGeneration() {
            if (coreGenerationCancellation != null) {
                coreGenerationCancellation.Cancel();
                coreGenerationCancellation = null;
                coreGenerationTask = null;
            }
        }


        // Built status check.

        private void UpdateWhenBuilt() {
            if (Built) {
                return;
            }
            if (core != null && (map == null || map.Built) && (unitSystem == null || unitSystem.Presentation != null)) {
                loadingStatus = "Ready!";
                Built = true;
            }
        }


    }

}

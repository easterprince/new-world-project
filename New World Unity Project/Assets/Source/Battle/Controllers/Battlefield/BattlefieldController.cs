using NewWorld.Battle.Controllers.Map;
using NewWorld.Battle.Controllers.UI.Loading;
using NewWorld.Battle.Controllers.UnitSystem;
using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Generation.Map;
using NewWorld.Battle.Cores.Generation.Units;
using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Battlefield {

    public class BattlefieldController : BuildableController {

        // Fields.

        private BattlefieldCore core = null;
        private bool paused = true;
        private string loadingStatus = "Waiting...";

        // Steady references.
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
            set {
                ValidateBeingNotFixed();
                map = value;
            }
        }

        public UnitSystemController UnitSystem {
            get => unitSystem;
            set {
                ValidateBeingNotFixed();
                unitSystem = value;
            }
        }

        public bool Paused {
            get => paused;
            set => paused = value;
        }


        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();
            GameObjects.ValidateReference(map, nameof(map));
            GameObjects.ValidateReference(unitSystem, nameof(unitSystem));
            ForceBeingStarted();

            // Start generating map.
            StartCoreGeneration();

        }

        private void Update() {
            
            // Check core generation progress.
            if (StartedBuilding && !FinishedBuilding) {
                if (coreGenerationTask != null && coreGenerationTask.IsCompleted) {

                    loadingStatus = "Drawing everything...";
                    core = coreGenerationTask.Result;
                    CancelCoreGeneration();

                    SetStartedBuilding();
                    map.StartBuilding(core.Map);
                    map.ExecuteWhenBuilt(this, UpdateIfBuilt);
                    unitSystem.Build(core.UnitSystem);
                    unitSystem.ExecuteWhenBuilt(this, UpdateIfBuilt);

                }
            }

            // Update core.
            if (FinishedBuilding && !paused) {
                float gameTimeDelta = Mathf.Min(Time.deltaTime, Time.fixedDeltaTime);
                core?.Update(gameTimeDelta);
            }

        }

        private protected override void OnDestroy() {
            CancelCoreGeneration();
            base.OnDestroy();
        }


        // Core generation.

        private void StartCoreGeneration() {
            ValidateBeingStarted();
            SetStartedBuilding();
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

        private void UpdateIfBuilt() {
            if (FinishedBuilding) {
                return;
            }
            if (core != null && map.FinishedBuilding && unitSystem.FinishedBuilding) {
                loadingStatus = "Ready!";
                SetFinishedBuilding();
            }
        }


    }

}

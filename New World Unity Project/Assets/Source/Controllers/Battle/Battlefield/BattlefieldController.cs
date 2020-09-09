using NewWorld.Controllers.Battle.Map;
using NewWorld.Controllers.Battle.UnitSystem;
using NewWorld.Cores.Battle.Battlefield;
using NewWorld.Cores.Battle.Generation.Map;
using NewWorld.Cores.Battle.Generation.Units;
using NewWorld.Cores.Battle.Layout;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Controllers.Battle.Battlefield {

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
        private Task<BattlefieldCore> coreGenerationTask = null;
        private object coreGenerationConditionLock = new object();
        private string coreGenerationCondition = null;
        private readonly CancellationTokenSource taskCancellation = new CancellationTokenSource();


        // Properties.

        public BattlefieldPresentation Presentation => core?.Presentation;

        public string LoadingStatus => loadingStatus;

        public MapController Map {
            get => map;
            set {
                ValidateBeingNotStarted();
                map = value;
            }
        }

        public UnitSystemController UnitSystem {
            get => unitSystem;
            set {
                ValidateBeingNotStarted();
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

            // Start generating core.
            SetStartedBuilding();
            coreGenerationCondition = "Generating game data...";
            var progress = new Progress<string>(condition => {
                lock (coreGenerationConditionLock) {
                    coreGenerationCondition = condition;
                }
            });
            coreGenerationTask = GenerateCoreAsync(progress, taskCancellation.Token);

        }

        private void Update() {

            void updateOnBuilding() {
                if (map.FinishedBuilding && unitSystem.FinishedBuilding) {
                    loadingStatus = "Ready!";
                    SetFinishedBuilding();
                }
            }

            // Check core generation progress.
            if (StartedBuilding && !FinishedBuilding && coreGenerationTask != null) {
                lock (coreGenerationConditionLock) {
                    loadingStatus = coreGenerationCondition;
                }
                if (coreGenerationTask.IsCompleted) {

                    // Get task result.
                    core = coreGenerationTask.Result;
                    coreGenerationTask = null;

                    // Start building dependent controllers.
                    loadingStatus = "Drawing everything...";
                    map.StartBuilding(core.Map);
                    map.ExecuteWhenBuilt(this, updateOnBuilding);
                    unitSystem.Build(core.UnitSystem);
                    unitSystem.ExecuteWhenBuilt(this, updateOnBuilding);

                }
            }

            // Update core.
            if (FinishedBuilding && !paused) {
                float gameTimeDelta = Mathf.Min(Time.deltaTime, Time.fixedDeltaTime);
                core?.Update(gameTimeDelta);
            }

        }

        private protected override void OnDestroy() {

            // Remove event handlers.
            if (map != null) {
                map.RemoveSubscriber(this);
            }
            if (unitSystem != null) {
                unitSystem.RemoveSubscriber(this);
            }

            // Cancel tasks.
            if (!taskCancellation.IsCancellationRequested) {
                taskCancellation.Cancel();
            }

            base.OnDestroy();
        }


        // Core generation.

        private static async Task<BattlefieldCore> GenerateCoreAsync(IProgress<string> progress, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            // Generate map.
            progress.Report("Generating map...");
            var mapGenerator = new FullOfHolesMapGenerator() {
                HeightLimit = 10,
                Size = new Vector2Int(100, 100)
            };
            var map = await mapGenerator.GenerateAsync(0, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            // Generate layout.
            progress.Report("Generating layout...");
            var layout = await LayoutCore.CreateLayoutAsync(map.Presentation, 5, 0.3f, 0.1f, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            // Generate unit system.
            progress.Report("Generating units...");
            var unitSystemGenerator = new UniformUnitSystemGenerator() {
                Map = map.Presentation,
                UnitCount = 60
            };
            var unitSystem = await unitSystemGenerator.GenerateAsync(0, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            // Assemble core.
            progress.Report("Finishing game data generation...");
            var core = new BattlefieldCore(map, layout, unitSystem);
            return core;
        }


    }

}

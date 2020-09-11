using NewWorld.Controllers.Battle.Map;
using NewWorld.Controllers.Battle.UnitSystem;
using NewWorld.Cores.Battle.Battlefield;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using NewWorld.Utilities.Events;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Controllers.Battle.Battlefield {

    public class BattlefieldController : BuildableController {

        // Enumerator.

        public enum BattleStatus {
            Inactive,
            Loading,
            Ready,
            Ongoing
        }


        // Fields.

        private BattlefieldCore core = null;
        private bool paused = true;
        private BattleStatus status = BattleStatus.Inactive;
        private string loadingStatus = "Waiting...";

        // Steady references.
        [SerializeField]
        private MapController map;
        [SerializeField]
        private UnitSystemController unitSystem;

        // Events.
        private ControllerEvent<BattleStatus> battleStatusChangedEvent = new ControllerEvent<BattleStatus>();
        private ControllerEvent<string> loadingStatusChangedEvent = new ControllerEvent<string>();

        // Tasks.
        private CancellationTokenSource taskCancellation = new CancellationTokenSource();


        // Properties.

        public BattlefieldPresentation Presentation => core?.Presentation;

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

        public string LoadingStatus {
            get => loadingStatus;
            private set {
                if (value == loadingStatus) {
                    return;
                }
                loadingStatus = value;
                loadingStatusChangedEvent.Invoke(loadingStatus);
            }
        }

        public bool Paused {
            get => paused;
            set {
                paused = value;
                if (FinishedBuilding) {
                    Status = (paused ? BattleStatus.Ready : BattleStatus.Ongoing);
                }
            }
        }

        public BattleStatus Status {
            get => status;
            private set {
                if (value == status) {
                    return;
                }
                status = value;
                battleStatusChangedEvent.Invoke(status);
            }
        }

        public ControllerEvent<BattleStatus>.EventWrapper BattleStatusChangedEvent => battleStatusChangedEvent.Wrapper;
        public ControllerEvent<string>.EventWrapper LoadingStatusChangedEvent => loadingStatusChangedEvent.Wrapper;


        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();
            GameObjects.ValidateReference(map, nameof(map));
            GameObjects.ValidateReference(unitSystem, nameof(unitSystem));

            // Start building.
            StartCoroutine(Build());

        }

        private void Update() {

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


        // Building coroutine.

        private IEnumerator Build() {
            SetStartedBuilding();

            // Start generating core.
            string coreGenerationCondition = "Obtaining game data...";
            object coreGenerationConditionLock = new object();
            var progress = new Progress<string>(condition => {
                lock (coreGenerationConditionLock) {
                    coreGenerationCondition = condition;
                }
            });
            var generateCoreAsync = GameManager.FinishBattleTransition();
            Task<BattlefieldCore> coreGenerationTask = generateCoreAsync(progress, taskCancellation.Token);

            // Check core generation progress.
            Status = BattleStatus.Loading;
            while (true) {
                if (!coreGenerationTask.IsCompleted) {

                    // Update loading status.
                    lock (coreGenerationConditionLock) {
                        LoadingStatus = coreGenerationCondition;
                    }

                    yield return null;

                } else {

                    // Get task result.
                    core = coreGenerationTask.Result;
                    if (core == null) {
                        LoadingStatus = "No core generated!";
                        Status = BattleStatus.Inactive;
                        break;
                    }

                    // Building finishing method.
                    void updateOnBuilding() {
                        if (map.FinishedBuilding && unitSystem.FinishedBuilding) {
                            LoadingStatus = "Ready!";
                            SetFinishedBuilding();
                            Status = BattleStatus.Ready;
                        }
                    }

                    // Start building dependent controllers.
                    LoadingStatus = "Drawing everything...";
                    map.StartBuilding(core.Map);
                    map.ExecuteWhenBuilt(this, updateOnBuilding);
                    unitSystem.Build(core.UnitSystem);
                    unitSystem.ExecuteWhenBuilt(this, updateOnBuilding);

                    break;

                }
            }

        }


    }

}

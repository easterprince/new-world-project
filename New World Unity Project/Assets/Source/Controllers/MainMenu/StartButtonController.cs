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
using UnityEngine.UI;

namespace NewWorld.Controllers.MainMenu {

    public class StartButtonController : SteadyController {

        // Fields.

        private Button button;


        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();
            button = GetComponent<Button>();
            GameObjects.ValidateComponent(button);
            button.onClick.AddListener(StartBattleScene);
        }

        private void OnDestroy() {
            button.onClick.RemoveListener(StartBattleScene);
        }


        // Battle loading.

        private void StartBattleScene() {
            if (GameManager.DoesTransit) {
                return;
            }
            GameManager.StartBattleTransition(GenerateCoreAsync);
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

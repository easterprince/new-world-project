using NewWorld.Cores.Battle.Battlefield;
using NewWorld.Cores.Battle.Generation.Map;
using NewWorld.Cores.Battle.Generation.Units;
using NewWorld.Cores.Battle.Layout;
using NewWorld.Cores.Battle.Unit;
using NewWorld.Cores.Battle.Unit.Abilities.Attacks;
using NewWorld.Cores.Battle.Unit.Abilities.Motions;
using NewWorld.Cores.Battle.Unit.Body;
using NewWorld.Cores.Battle.Unit.Durability;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System;
using System.Collections.Generic;
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
                UnitCount = 60,
                UnitTemplates = GenerateUnitTemplates()
            };
            var unitSystem = await unitSystemGenerator.GenerateAsync(0, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            // Assemble core.
            progress.Report("Finishing game data generation...");
            var core = new BattlefieldCore(map, layout, unitSystem);
            return core;
        }

        private static UnitCore[] GenerateUnitTemplates() {
            var templates = new List<UnitCore>();

            {
                var template = new UnitCore(
                    body: new BodyModule() {
                        CollidesWith = BodyModule.CollisionMode.Surface
                    },
                    durability: new DurabilityModule(
                        idleConditionId: NamedId.Get("SimpleIdle"),
                        collapseConditionId: NamedId.Get("SimpleCollapse"),
                        durabilityLimit: 200
                    )
                );
                template.AddAbility(new DirectAttackAbility(
                    singleAttackDamage: new Damage(50),
                    attackDuration: 1f,
                    attackMoment: 0.5f,
                    attackRange: 1f,
                    conditionId: NamedId.Get("SimpleAttack")
                ));
                template.AddAbility(new DirectMotionAbility(
                    id: NamedId.Get("SimpleMotion"),
                    speed: 1f
                ));
                templates.Add(template);
            }

            {
                var template = new UnitCore(
                    body: new BodyModule() {
                        CollidesWith = BodyModule.CollisionMode.Surface
                    },
                    durability: new DurabilityModule(
                        idleConditionId: NamedId.Get("SimpleIdle"),
                        collapseConditionId: NamedId.Get("SimpleCollapse"),
                        durabilityLimit: 100
                    )
                );
                template.AddAbility(new DirectAttackAbility(
                    singleAttackDamage: new Damage(50),
                    attackDuration: 1f,
                    attackMoment: 0.5f,
                    attackRange: 1f,
                    conditionId: NamedId.Get("SimpleAttack")
                ));
                template.AddAbility(new DirectMotionAbility(
                    id: NamedId.Get("SimpleMotion"),
                    speed: 1f
                ));
                templates.Add(template);
            }

            return templates.ToArray();
        }


    }

}

using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.Unit;
using NewWorld.Battle.Cores.Unit.Abilities.Attacks;
using NewWorld.Battle.Cores.Unit.Abilities.Motions;
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.Unit.Durability;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities;
using System;
using System.Threading;
using UnityEngine;

namespace NewWorld.Battle.Cores.Generation.Units {

    public class UniformUnitSystemGenerator : UnitSystemGenerator {
        
        // Generation.
        
        public override UnitSystemCore Generate(int seed, CancellationToken? cancellationToken = null) {
            var unitSystem = new UnitSystemCore();
            int added = 0;

            cancellationToken?.ThrowIfCancellationRequested();

            // Generate unit template.
            var template = new UnitCore(
                body: new BodyModule() {
                    CollidesWith = BodyModule.CollisionMode.Surface
                },
                durability: new DurabilityModule() {
                    DurabilityLimit = 300,
                    Durability = 250
                }
            );
            var attack = new DirectAttackAbility() {
                SingleAttackDamage = new Damage(50)
            };
            template.AddAbility(attack);
            var motion = new DirectMotionAbility();
            template.AddAbility(motion);

            // Count free nodes.
            int freeNodes = 0;
            foreach (var position in Enumerables.InRange2(Map.Size)) {
                if (Map[position].Type == MapNode.NodeType.Common) {
                    ++freeNodes;
                }
            }
            if (freeNodes <= UnitCount) {
                foreach (var position in Enumerables.InRange2(Map.Size)) {
                    if (Map[position].Type == MapNode.NodeType.Common) {
                        unitSystem.AddUnit(BuildUnit(template, position, Map, ref added), position);
                    }
                    cancellationToken?.ThrowIfCancellationRequested();
                }
                return unitSystem;
            }
            cancellationToken?.ThrowIfCancellationRequested();

            // Place units at free nodes.
            var random = new System.Random(seed);
            int toAdd = UnitCount;
            while (added < UnitCount) {
                Vector2Int position;
                do {
                    position = new Vector2Int(random.Next(Map.Size.x), random.Next(Map.Size.y));
                    cancellationToken?.ThrowIfCancellationRequested();
                } while (Map[position].Type != MapNode.NodeType.Common || unitSystem[position] != null);
                unitSystem.AddUnit(BuildUnit(template, position, Map, ref added), position);
                cancellationToken?.ThrowIfCancellationRequested();
            }
            return unitSystem;
        }


        // Support method.

        private UnitCore BuildUnit(UnitCore template, Vector2Int position, MapPresentation map, ref int added) {
            var unit = template.Clone();
            unit.Name = $"Unit {added++}";
            Vector3 point = new Vector3(position.x, 0, position.y);
            point.y = map.GetHeight(point);
            unit.Move(new MovementAction(point - unit.Body.Position, adjustVelocity: false));
            return unit;
        }


    }

}

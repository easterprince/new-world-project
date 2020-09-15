using NewWorld.Cores.Battle.Map;
using NewWorld.Cores.Battle.Unit;
using NewWorld.Cores.Battle.Unit.Abilities.Attacks;
using NewWorld.Cores.Battle.Unit.Abilities.Motions;
using NewWorld.Cores.Battle.Unit.Body;
using NewWorld.Cores.Battle.Unit.Durability;
using NewWorld.Cores.Battle.UnitSystem;
using NewWorld.Utilities;
using System.Threading;
using UnityEngine;

namespace NewWorld.Cores.Battle.Generation.Units {

    public class UniformUnitSystemGenerator : UnitSystemGenerator {

        // Generation.

        public override UnitSystemCore Generate(int seed, CancellationToken? cancellationToken = null) {
            var unitSystem = new UnitSystemCore();
            int added = 0;

            cancellationToken?.ThrowIfCancellationRequested();

            // Initialize random.
            var random = new System.Random(seed);

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
                        unitSystem.AddUnit(BuildUnit(random, position, Map, ref added), position);
                    }
                    cancellationToken?.ThrowIfCancellationRequested();
                }
                return unitSystem;
            }
            cancellationToken?.ThrowIfCancellationRequested();

            // Place units at free nodes.
            int toAdd = UnitCount;
            while (added < UnitCount) {
                Vector2Int position;
                do {
                    position = new Vector2Int(random.Next(Map.Size.x), random.Next(Map.Size.y));
                    cancellationToken?.ThrowIfCancellationRequested();
                } while (Map[position].Type != MapNode.NodeType.Common || unitSystem[position] != null);
                unitSystem.AddUnit(BuildUnit(random, position, Map, ref added), position);
                cancellationToken?.ThrowIfCancellationRequested();
            }
            return unitSystem;
        }


        // Support method.

        private UnitCore BuildUnit(System.Random random, Vector2Int position, MapPresentation map, ref int added) {

            // Initialize unit.
            UnitCore unit;
            if (UnitTemplates == null || UnitTemplates.Length == 0) {
                unit = new UnitCore();
            } else {
                unit = UnitTemplates[random.Next(UnitTemplates.Length)].Clone();
            }

            // Place unit.
            unit.Name = $"Unit {added++}";
            Vector3 point = new Vector3(position.x, map[position].Height, position.y);
            unit.Move(new MovementAction(point - unit.Body.Position, adjustVelocity: false));
            return unit;
        }


    }

}

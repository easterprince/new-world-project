using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.Unit;
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Generation.Units {

    public class UniformUnitSystemGenerator : UnitSystemGenerator {
        
        // Generation.
        
        public override UnitSystemCore Generate(int seed, MapCore map) {
            var unitSystem = new UnitSystemCore();
            int added = 0;

            // Generate unit template.
            var template = new UnitCore();

            // Count free nodes.
            int freeNodes = 0;
            foreach (var position in Enumerables.InRange2(map.Size)) {
                if (map[position].Type == MapNode.NodeType.Common) {
                    ++freeNodes;
                }
            }
            if (freeNodes >= UnitCount) {
                foreach (var position in Enumerables.InRange2(map.Size)) {
                    if (map[position].Type == MapNode.NodeType.Common) {
                        unitSystem.AddUnit(BuildUnit(template, position, map.Presentation, ref added), position);
                    }
                }
                return unitSystem;
            }

            // Place units at free nodes.
            var random = new System.Random(seed);
            int toAdd = UnitCount;
            while (added < UnitCount) {
                Vector2Int position;
                do {
                    position = new Vector2Int(random.Next(map.Size.x), random.Next(map.Size.y));
                } while (map[position].Type != MapNode.NodeType.Common || unitSystem[position] != null);
                unitSystem.AddUnit(BuildUnit(template, position, map.Presentation, ref added), position);
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

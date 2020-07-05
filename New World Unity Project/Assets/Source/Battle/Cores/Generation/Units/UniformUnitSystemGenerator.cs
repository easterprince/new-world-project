using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.Unit;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Generation.Units {

    public class UniformUnitSystemGenerator : UnitSystemGenerator {
        
        // Generation.
        
        public override UnitSystemCore Generate(int seed, MapCore map) {
            var unitSystem = new UnitSystemCore();

            // Generate unit template.
            var unit = new UnitCore();

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
                        unitSystem.AddUnit(unit, position);
                    }
                }
                return unitSystem;
            }

            // Place units at free nodes.
            var random = new System.Random(seed);
            int toAdd = UnitCount;
            for (int added = 0; added < UnitCount; ++added) {
                Vector2Int position;
                do {
                    position = new Vector2Int(random.Next(map.Size.x), random.Next(map.Size.y));
                } while (map[position].Type != MapNode.NodeType.Common || unitSystem[position] != null);
                unitSystem.AddUnit(unit, position);
            }
            return unitSystem;
        }


    }

}

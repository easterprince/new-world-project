using System;
using UnityEngine;
using Unity.Mathematics;
using NewWorld.Cores.Battle.Map;
using System.Linq;
using NewWorld.Utilities;
using System.Threading;

namespace NewWorld.Cores.Battle.Generation.Map {

    public class FullOfHolesMapGenerator : MapGenerator {

        private static double Erf(double x) {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }

        private static float ShiftedErf(float x) {
            return ((float) Erf(4 * x - 2) + 1) / 2;
        }

        public override MapCore Generate(int seed, CancellationToken? cancellationToken = null) {

            cancellationToken?.ThrowIfCancellationRequested();

            var map = new MapCore(Size, HeightLimit); 
            foreach (var position in Enumerables.InRange2(Size)) {
                float noiseValue = noise.cellular2x2(0.05f * new float2(position.x, position.y)).x;
                float normalizedHeight = (noiseValue - 0.4f) / (1 - 0.4f);
                if (normalizedHeight >= 0) {
                    float height = 2 * ShiftedErf(normalizedHeight);
                    var node = new MapNode(MapNode.NodeType.Common, height);
                    map[position] = node;
                }

                cancellationToken?.ThrowIfCancellationRequested();

            }
            return map;
        }

    }

}

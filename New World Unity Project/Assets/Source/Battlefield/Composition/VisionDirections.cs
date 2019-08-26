using System;
using UnityEngine;

namespace NewWorld.Battlefield.Composition {

    public static class VisionDirections {

        // Fields.

        private const int count = 4;


        // Properties.

        public static int Count => count;


        // Directions processing.

        public static bool IsValidDirection(int visionDirection) {
            return 0 <= visionDirection && visionDirection < Count;
        }

        public static Vector2Int GetDirectionDelta(int visionDirection) {
            if (!IsValidDirection(visionDirection)) {
                throw BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            return new Vector2Int((visionDirection == 0 || visionDirection == 2 ? 1 : -1), (visionDirection == 0 || visionDirection == 1 ? 1 : -1));
        }

        public static int GetNextClockwiseDirection(int visionDirection) {
            if (!IsValidDirection(visionDirection)) {
                throw BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            if (visionDirection == 0 || visionDirection == 3) {
                return visionDirection ^ 2;
            } else {
                return visionDirection ^ 1;
            }
        }

        public static int GetNextCounterclockwiseDirection(int visionDirection) {
            if (!IsValidDirection(visionDirection)) {
                throw BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            if (visionDirection == 0 || visionDirection == 3) {
                return visionDirection ^ 1;
            } else {
                return visionDirection ^ 2;
            }
        }

        public static int GetOppositeDirection(int visionDirection) {
            if (!IsValidDirection(visionDirection)) {
                throw BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            return visionDirection ^ 3;
        }


        // Exception builder.

        public static ArgumentOutOfRangeException BuildInvalidDirectionException(string parameterName, int parameterValue) {
            return new ArgumentOutOfRangeException(
                parameterName,
                parameterValue,
                $"Provided direction is invalid. It must be non-negative integer less than {Count}."
            );
        }

    }

}

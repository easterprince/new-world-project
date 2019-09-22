using System;
using UnityEngine;

namespace NewWorld.Battlefield.Composition {

    public static class CoordinatesTransformations {

        // Fields.

        private const float tileHidingHeightDifference = 0.6123724356957945245493210f; // = sqrt(6) / 4.
        private const float reversedHidingDifference = 1.63299316185545206546485604980f; // = 4 / sqrt(6).


        // Properties.

        public static float TileHidingHeightDifference => tileHidingHeightDifference;


        // Coordinates transformations.

        public static Vector3 RealToVisible(Vector3 real, int visionDirection, out int order) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException(nameof(visionDirection), visionDirection);
            }
            return RealToVisible(real, visionDirection, 0, out order);
        }

        public static Vector3 RealToVisible(Vector3 real, int visionDirection) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException(nameof(visionDirection), visionDirection);
            }
            return RealToVisible(real, visionDirection, 0);
        }

        public static Vector3 RealToVisible(Vector3 real, int visionDirection, float size, out int order) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException(nameof(visionDirection), visionDirection);
            }
            if (!IsValidSize(size)) {
                throw BuildInvalidSizeException(nameof(size), size);
            }
            return RotatedToVisible(RotateReal(real, visionDirection), size, out order);
        }

        public static Vector3 RealToVisible(Vector3 real, int visionDirection, float size) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException(nameof(visionDirection), visionDirection);
            }
            if (!IsValidSize(size)) {
                throw BuildInvalidSizeException(nameof(size), size);
            }
            return RotatedToVisible(RotateReal(real, visionDirection), size);
        }


        // Exception builder.

        public static bool IsValidSize(float parameterValue) {
            return parameterValue >= 0;
        }

        public static ArgumentOutOfRangeException BuildInvalidSizeException(string parameterName, float parameterValue) {
            return new ArgumentOutOfRangeException(
                parameterName,
                parameterValue,
                $"Provided size is invalid. It must be non-negative."
            );
        }


        // Support.

        private static Vector3 RotateReal(Vector3 real, int visionDirection) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException(nameof(visionDirection), visionDirection);
            }
            Vector3 rotated = real;
            if (visionDirection == 1 || visionDirection == 2) {
                rotated.x = real.y;
                rotated.y = real.x;
            }
            if (visionDirection == 2 || visionDirection == 3) {
                rotated.x = -rotated.x;
            }
            if (visionDirection == 1 || visionDirection == 3) {
                rotated.y = -rotated.y;
            }
            return rotated;
        }

        private static Vector3 RotatedToVisible(Vector3 real, float size, out int order) {
            if (!IsValidSize(size)) {
                throw BuildInvalidSizeException(nameof(size), size);
            }
            order = -Mathf.RoundToInt(2 * (real.x + real.y - size)) * SpriteLayers.SublayersPerLayer;
            return RotatedToVisible(real, size);
        }

        private static Vector3 RotatedToVisible(Vector3 real, float size) {
            if (!IsValidSize(size)) {
                throw BuildInvalidSizeException(nameof(size), size);
            }
            return new Vector3(
                real.x - real.y,
                0.5f * real.x + 0.5f * real.y + reversedHidingDifference * real.z,
                -reversedHidingDifference * real.z
            );
        }

    }

}

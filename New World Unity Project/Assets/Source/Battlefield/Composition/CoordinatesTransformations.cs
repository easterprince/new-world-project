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
                throw VisionDirections.BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            return RotatedToVisible(RotateReal(real, visionDirection), out order);
        }

        public static Vector3 RealToVisible(Vector3 real, int visionDirection) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException("visionDirection", visionDirection);
            }
            return RotatedToVisible(RotateReal(real, visionDirection));
        }


        // Support.

        private static Vector3 RotateReal(Vector3 real, int visionDirection) {
            if (!VisionDirections.IsValidDirection(visionDirection)) {
                throw VisionDirections.BuildInvalidDirectionException("visionDirection", visionDirection);
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

        private static Vector3 RotatedToVisible(Vector3 real, out int order) {
            order = -Mathf.RoundToInt(2 * real.x + 2 * real.y) * SpriteLayers.SublayersPerLayer;
            return RotatedToVisible(real);
        }

        private static Vector3 RotatedToVisible(Vector3 real) {
            return new Vector3(
                real.x - real.y,
                0.5f * real.x + 0.5f * real.y + reversedHidingDifference * real.z,
                -reversedHidingDifference * real.z
            );
        }

    }

}

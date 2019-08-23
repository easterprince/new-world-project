using UnityEngine;

namespace NewWorld.Battlefield {

    public static class BattlefieldComposition {

        // Enumerator.

        public enum Sublayers {
            TilesBackground = 0,
            TilesForeground
        }


        // Fields.

        private const int visionDirectionsCount = 4;

        private const int sublayersPerLayer = 10;

        private const float tileHidingHeightDifference = 0.6123724356957945245493210f; // = sqrt(6) / 4.
        private const float reversedHidingDifference = 1.63299316185545206546485604980f; // = 4 / sqrt(6).


        // Properties.

        public static int VisionDirectionsCount => visionDirectionsCount;

        public static int SublayersPerLayer => sublayersPerLayer;

        public static float TileHidingHeightDifference => tileHidingHeightDifference;


        // Directions processing.

        public static Vector2Int GetDirectionDelta(int direction) {
            return new Vector2Int((direction == 0 || direction == 2 ? 1 : -1), (direction == 0 || direction == 1 ? 1 : -1));
        }

        public static int GetNextClockwiseDirection(int direction) {
            if (direction == 0 || direction == 3) {
                return direction ^ 2;
            } else {
                return direction ^ 1;
            }
        }

        public static int GetNextCounterclockwiseDirection(int direction) {
            if (direction == 0 || direction == 3) {
                return direction ^ 1;
            } else {
                return direction ^ 2;
            }
        }

        public static int GetOppositeDirection(int direction) {
            return direction ^ 3;
        }


        // Getting coordinates for positioning of sprites.

        public static Vector3 RealToVisible(Vector3 real, int direction, out int order) {
            return RotatedToVisible(RotateReal(real, direction), out order);
        }

        public static Vector3 RealToVisible(Vector3 real, int direction) {
            return RotatedToVisible(RotateReal(real, direction));
        }

        private static Vector3 RotateReal(Vector3 real, int direction) {
            Vector3 rotated = real;
            if (direction == 1 || direction == 2) {
                rotated.x = real.y;
                rotated.y = real.x;
            }
            if (direction == 2 || direction == 3) {
                rotated.x = -rotated.x;
            }
            if (direction == 1 || direction == 3) {
                rotated.y = -rotated.y;
            }
            return rotated;
        }

        private static Vector3 RotatedToVisible(Vector3 real, out int order) {
            order = -Mathf.RoundToInt(2 * real.x + 2 * real.y) * sublayersPerLayer;
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

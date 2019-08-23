using UnityEngine;

namespace NewWorld.BattleField.Map {

    public static class VisionDirections {

        // Fields.

        private const int count = 4;


        // Properties.

        public static int Count => count;


        // Methods.

        public static Vector2Int GetDelta(int direction) {
            return new Vector2Int((((direction & 1) == 0) ? 1 : -1), (((direction & 2) == 0) ? 1 : -1));
        }

        public static int GetNextClockwiseDirection(int direction) {
            if (((direction & 1) == ((direction & 2) >> 1))) {
                return direction ^ 2;
            } else {
                return direction ^ 1;
            }
        }

        public static int GetNextCounterclockwiseDirection(int direction) {
            if (((direction & 1) == ((direction & 2) >> 1))) {
                return direction ^ 1;
            } else {
                return direction ^ 2;
            }
        }

        public static int GetOppositeDirection(int direction) {
            return direction ^ 3;
        }

    }

}

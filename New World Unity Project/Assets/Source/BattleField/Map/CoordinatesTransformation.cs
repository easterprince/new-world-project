using UnityEngine;

namespace NewWorld.BattleField.Map {

    public static class CoordinatesTransformation {

        // Fields.

        private const float hidingDifference = 0.6123724356957945245493210f; // = sqrt(6) / 4.
        private const float reversedHidingDifference = 1.63299316185545206546485604980f; // = 4 / sqrt(6).


        // Properties.

        public static float HidingDifference => hidingDifference;


        // Methods.

        public static Vector3 RealToVisible(Vector3 real) {
            return new Vector3(
                real.x - real.y,
                0.5f * real.x + 0.5f * real.y + reversedHidingDifference * real.z,
                -reversedHidingDifference * real.z
            );
        }

        public static Vector3 RealToVisible(Vector3 real, out int order) {
            order = -Mathf.RoundToInt(2 * real.x + 2 * real.y) * SpriteLayers.SublayersPerLayer;
            return RealToVisible(real);
        }

    }

}

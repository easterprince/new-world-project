using UnityEngine;

namespace NewWorld.BattleField {

    static class SpriteLayers {

        // Enumerator.

        public enum Sublayers {
            TilesBackground = 0,
            TilesForeground
        }


        // Fields.

        private const int sublayersPerLayer = 10;


        // Properties.

        public static int SublayersPerLayer => sublayersPerLayer;


    }

}

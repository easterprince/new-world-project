using UnityEngine;

namespace NewWorld.Battlefield.Composition {

    public static class SpriteLayers {

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

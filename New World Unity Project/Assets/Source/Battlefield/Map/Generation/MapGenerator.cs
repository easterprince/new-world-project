using UnityEngine;
using Unity.Mathematics;
using NewWorld.Battlefield.Map;

namespace NewWorld.Battlefield.Map.Generation {

    public abstract class MapGenerator {

        // Fields.

        private int seed = 0;
        private float heightLimit = 0;
        private Vector2Int size = new Vector2Int(1, 1);


        // Constructor.

        public MapGenerator() {}


        // Properties.

        public int Seed {
            get => seed;
            set => seed = value;
        }

        public float HeightLimit {
            get => heightLimit;
            set => heightLimit = value;
        }

        public Vector2Int Size {
            get => size;
            set => size = value;
        }


        // Methods.

        public abstract MapDescription Generate();


    }

}

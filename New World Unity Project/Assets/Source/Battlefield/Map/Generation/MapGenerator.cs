using UnityEngine;
using Unity.Mathematics;
using NewWorld.Battlefield.Map;

namespace NewWorld.Battlefield.Map.Generation {

    public abstract class MapGenerator {

        // Fields.

        private int seed;
        private float heightLimit;
        private Vector2 size;


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

        public Vector2 Size {
            get => size;
            set => size = value;
        }


        // Methods.

        public abstract MapDescription Generate();


    }

}

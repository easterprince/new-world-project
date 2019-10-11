using UnityEngine;

namespace NewWorld.Battlefield.Map.Generation {

    public abstract class MapGenerator {

        // Fields.

        private float heightLimit = 0;
        private Vector2Int size = new Vector2Int(1, 1);


        // Constructor.

        public MapGenerator() {}


        // Properties.

        public float HeightLimit {
            get => heightLimit;
            set => heightLimit = value;
        }

        public Vector2Int Size {
            get => size;
            set => size = value;
        }


        // Methods.

        public abstract MapDescription Generate(int seed);


    }

}

using UnityEngine;
using Unity.Mathematics;
using NewWorld.Battlefield.Map;

namespace NewWorld.Battlefield.Map.Generation {

    public class ExperimentalMapGenerator : MapGenerator {

        public override MapDescription Generate() {
            MapDescription description = new MapDescription(Size, HeightLimit);
            System.Random random = new System.Random(Seed);
            for (Vector2Int position = Vector2Int.zero; position.x < Size.x; ++position.x) {
                for (position.y = 0; position.y < Size.y; ++position.y) {
                    float height = noise.cellular2x2(new float2(position.x, position.y)).x;
                    description.SetSurfaceNode(position, new NodeDescription(height));
                }
            }
            return description;
        }

    }

}

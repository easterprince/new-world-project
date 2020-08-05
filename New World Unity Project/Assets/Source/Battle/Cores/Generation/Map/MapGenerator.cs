using NewWorld.Battle.Cores.Map;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Battle.Cores.Generation.Map {

    public abstract class MapGenerator {

        // Fields.

        private float heightLimit = 0;
        private Vector2Int size = Vector2Int.zero;


        // Properties.

        public float HeightLimit {
            get => heightLimit;
            set => heightLimit = value;
        }

        public Vector2Int Size {
            get => size;
            set => size = Vector2Int.Max(value, Vector2Int.zero);
        }


        // Methods.

        public abstract MapCore Generate(int seed, CancellationToken? cancellationToken = null);

        public async Task<MapCore> GenerateAsync(int seed, CancellationToken? cancellationToken = null) {
            cancellationToken?.ThrowIfCancellationRequested();
            var task = Task.Run(() => Generate(seed, cancellationToken));
            MapCore map = await task;
            return map;
        }


    }

}

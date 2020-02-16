using System.Collections;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Loading;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Map {

    public class MapController : SceneSingleton<MapController> {

        // Fields.

        private NodeDescription[,] surface = new NodeDescription[0, 0];


#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private TerrainController terrain;

#pragma warning restore IDE0044, CS0414, CS0649


        // Properties.

        public Vector2Int Size => new Vector2Int(surface.GetLength(0), surface.GetLength(1));

        public NodeDescription this[Vector2Int position] {
            get {
                if (surface.Length == 0) {
                    return new NodeDescription();
                }
                position = GetNearestRealNodePosition(position);
                return surface[position.x, position.y];
            }
        }


        // Life cycle.

        override protected void Awake() {
            base.Awake();
            Instance = this;
        }


        // Initialization.

        public IEnumerator Load(MapDescription description) {
            if (description == null) {
                yield break;
            }
            surface = new NodeDescription[description.Size.x, description.Size.y];
            foreach (var position in Enumerables.InRange2(Size)) {
                surface[position.x, position.y] = description[position];
            }
            yield return StartCoroutine(terrain.Load(description));
        }


        // Information.

        public float GetSurfaceHeight(Vector2 position) {
            return terrain.GetSurfaceHeight(position);
        }

        public float GetSurfaceHeight(Vector3 position) {
            return terrain.GetSurfaceHeight(position);
        }


        // Support.

        private bool IsRealNodePosition(Vector2Int position) {
            return position.x >= 0 && position.x < Size.x && position.y >= 0 && position.y < Size.y;
        }
        
        private Vector2Int GetNearestRealNodePosition(Vector2 position) {
            position.x = Mathf.Clamp(position.x, 0, Size.x - 1);
            position.y = Mathf.Clamp(position.y, 0, Size.y - 1);
            return Vector2Int.RoundToInt(position);
        }


    }

}

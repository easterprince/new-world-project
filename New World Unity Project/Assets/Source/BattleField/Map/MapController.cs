using System.Collections;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Loading;

namespace NewWorld.Battlefield.Map {

    public partial class MapController : SceneSingleton<MapController> {

        // Fields.

        private MapDescription description;


#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private TerrainController terrain;

        [SerializeField]
        private NodeGridController nodeGrid;

#pragma warning restore IDE0044, CS0414, CS0649


        // Properties.

        public Vector2Int Size => description.Size;
        public float HeightLimit => description.HeightLimit;


        // Life cycle.

        override protected void Awake() {
            base.Awake();
            Instance = this;
        }


        // Initialization.

        public IEnumerator Load(MapDescription description) {
            this.description = description ?? throw new System.ArgumentNullException(nameof(description));
            yield return StartCoroutine(terrain.Load(description));
            yield return StartCoroutine(nodeGrid.Load(description));
        }


        // Information.

        public float GetSurfaceHeight(Vector2 position, float maximumRadius = 0) {
            if (maximumRadius < 0) {
                throw new System.ArgumentOutOfRangeException(nameof(maximumRadius), "Radius should be non-negative number.");
            }
            return terrain.GetSurfaceHeight(position, maximumRadius);
        }

        public float GetSurfaceHeight(Vector3 position, float maximumRadius = 0) {
            if (maximumRadius < 0) {
                throw new System.ArgumentOutOfRangeException(nameof(maximumRadius), "Radius should be non-negative number.");
            }
            return terrain.GetSurfaceHeight(position, maximumRadius);
        }

        public NodeDescription GetSurfaceNode(Vector2Int position) {
            return description.GetSurfaceNode(position);
        }


        // Support.

        private bool IsValidNodePosition(Vector2Int parameterValue) {
            return parameterValue.x >= 0 && parameterValue.x < description.Size.x && parameterValue.y >= 0 && parameterValue.y < description.Size.y;
        }

        private System.ArgumentOutOfRangeException BuildInvalidNodePositionException(string parameterName, Vector2Int parameterValue) {
            return new System.ArgumentOutOfRangeException(
                parameterName,
                parameterValue,
                $"Position must be inside node array, which size is {description.Size}."
            );
        }


    }

}

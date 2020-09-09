using UnityEngine;

namespace NewWorld.Utilities {

    public static class Positions {

        public static Vector2Int WorldToNode(Vector2 worldPosition) {
            return Vector2Int.RoundToInt(worldPosition);
        }

        public static Vector2Int WorldToNode(Vector3 worldPosition) {
            return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.z));
        }

        public static Vector2 NodeToWorld(Vector2Int nodePosition) {
            return nodePosition;
        }

        public static Vector3 NodeToWorld(Vector2Int nodePosition, float height) {
            return new Vector3(nodePosition.x, height, nodePosition.y);
        }


    }

}

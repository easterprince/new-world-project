using NewWorld.Battlefield.Map;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public class NodeController : MonoBehaviour {

        // Fabric.

        private const string prefabPath = "Prefabs/Node";
        private static GameObject prefab;

        public static NodeController BuildNode(Vector2Int position) {
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject nodeObject = Instantiate(prefab);
            NodeController node = nodeObject.GetComponent<NodeController>();
            node.Position = position;
            return node;
        }


        // Fields.

        private Vector2Int position;


        // Properties.

        public Vector2Int Position {
            get => position;
            set {
                position = value;
                float y = MapController.Instance.GetSurfaceHeight(position);
                transform.position = new Vector3(position.x, y, position.y);
                name = $"Node ({position.x}, {position.y})";
            }
        }

        public Color Color {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }


        // Life cycle.

        private void Awake() {
            MapController.EnsureInstance(this);
        }


    }

}

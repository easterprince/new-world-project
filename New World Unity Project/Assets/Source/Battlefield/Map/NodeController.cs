using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Map {

    public class NodeController : MonoBehaviour {

        // Fabric.

        private const string prefabPath = "Prefabs/Node";
        private const string defaultGameobjectName = "Node";
        private static GameObject prefab;

        public static NodeController BuildNode(string name = defaultGameobjectName) {
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject node = Instantiate(prefab);
            node.name = name ?? defaultGameobjectName;
            NodeController nodeController = node.GetComponent<NodeController>();
            return nodeController;
        }


        // Life cycle.

        private void Awake() {}


        // External control.

        public void Place(Vector3 position) {
            transform.position = position;
        }
               
    }

}

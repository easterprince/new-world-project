using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Composition;

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


        // Fields.

        private Vector3 realPosition;
        private int currentVisionDirection;

        private SpriteRenderer spriteRenderer;


        // Properties.

        public Vector3 RealPosition => realPosition;


        // Life cycle.

        private void Awake() {
            realPosition = Vector3.zero;
            currentVisionDirection = 0;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        // External control.

        public void Place(Vector3 newRealPosition) {
            realPosition = newRealPosition;
            UpdateVisiblePosition();
        }

        public void Rotate(int newVisionDirection) {
            currentVisionDirection = newVisionDirection;
            UpdateVisiblePosition();
        }


        // Support.

        private void UpdateVisiblePosition() {
            transform.position = CoordinatesTransformations.RealToVisible(realPosition, currentVisionDirection, out int spriteOrder);
            spriteRenderer.sortingOrder = spriteOrder + (int) SpriteLayers.Sublayers.Nodes;
        }


    }

}

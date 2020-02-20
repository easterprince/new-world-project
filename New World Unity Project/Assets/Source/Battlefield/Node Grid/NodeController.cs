using NewWorld.Battlefield.Map;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.NodeGrid {

    public class NodeController : MonoBehaviour {

        // Fabric.

        private const string prefabPath = "Prefabs/Node";
        private static GameObject prefab;

        public static NodeController BuildNode(Vector2Int position, Transform parent = null) {
            if (prefab == null) {
                prefab = Resources.Load<GameObject>(prefabPath);
            }
            GameObject nodeObject = Instantiate(prefab);
            NodeController node = nodeObject.GetComponent<NodeController>();
            node.Position = position;
            if (parent != null) {
                node.transform.parent = parent;
            }
            return node;
        }


        // Static.

        private static MaterialPropertyBlock materialProperties;


        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private MeshRenderer meshRenderer;

#pragma warning restore IDE0044, CS0414, CS0649

        private Vector2Int position;


        // Properties.

        public bool Shown {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

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
            set {
                materialProperties.SetColor("_Color", value);
                meshRenderer.SetPropertyBlock(materialProperties);
            }
        }


        // Life cycle.

        private void Awake() {
            MapController.EnsureInstance(this);
            if (materialProperties == null) {
                materialProperties = new MaterialPropertyBlock();
            }
            if (meshRenderer == null) {
                throw new MissingComponentException($"Need {typeof(MeshRenderer)}!");
            }
        }


    }

}

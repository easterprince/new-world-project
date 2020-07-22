using NewWorld.Battle.Controllers.Map;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Nodes {

    public class NodeController : MonoBehaviour {

        // Fabric.

        private static GameObject Prefab => PrefabSourceController.Instance.NodePrefab;

        public static NodeController BuildNode(MapController map, Vector2Int position) {
            GameObject node = Instantiate(Prefab);
            NodeController controller = node.GetComponent<NodeController>();
            controller.Map = map;
            controller.Position = position;
            return controller;
        }


        // Fields.

        private Vector2Int position = new Vector2Int();
        private MaterialPropertyBlock materialProperties;

        // References.
        [SerializeField]
        private MapController map;

        // Steady references.
        [SerializeField]
        private MeshRenderer meshRenderer;


        // Properties.

        public MapController Map {
            get => map;
            set => map = value;
        }

        public bool Shown {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public Vector2Int Position {
            get => position;
            set {
                position = value;
                float y = 0;
                if (map != null && map.Presentation != null) {
                    y = map.Presentation[position].Height;
                }
                transform.position = new Vector3(position.x, y, position.y);
                name = $"Node ({position.x}, {position.y})";
            }
        }

        public Color Color {
            get => materialProperties.GetColor("_Color");
            set {
                materialProperties.SetColor("_Color", value);
                meshRenderer.SetPropertyBlock(materialProperties);
            }
        }


        // Life cycle.

        private void Awake() {
            materialProperties = new MaterialPropertyBlock();
            GameObjects.ValidateReference(meshRenderer, nameof(meshRenderer));
        }


    }

}

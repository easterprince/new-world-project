using UnityEngine;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Map {

    public class NodeGridController : MonoBehaviour {

        // Fields.

        private NodeController[,] nodes;


        // Life cycle.

        void Awake() {}


        // Control.

        public void Load(MapDescription mapDescription) {
            nodes = new NodeController[mapDescription.Size.x, mapDescription.Size.y];
            for (int x = 0; x < mapDescription.Size.x; ++x) {
                for (int y = 0; y < mapDescription.Size.y; ++y) {
                    NodeDescription nodeDescription = mapDescription.GetSurfaceNode(new Vector2Int(x, y));
                    if (nodeDescription == null) {
                        continue;
                    }
                    NodeController node = NodeController.BuildNode($"Node ({x}, {y})");
                    node.transform.parent = transform;
                    nodes[x, y] = node;
                    //float height = nodeDescription.Height;
                    float height = MapController.Instance.GetSurfaceHeight(new Vector2(x, y));
                    Vector3 position = new Vector3(x, height, y);
                    node.Place(position);
                }
            }
        }


    }

}

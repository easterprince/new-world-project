using System.Collections;
using UnityEngine;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield.Map {

    public class NodeGridController : MonoBehaviour {

        // Fields.

        private NodeController[,] nodes;


        // Life cycle.

        void Awake() {}


        // Control.

        public IEnumerator Load(MapDescription description) {
            if (description == null) {
                throw new System.ArgumentNullException(nameof(description));
            }

            nodes = new NodeController[description.Size.x, description.Size.y];
            for (int x = 0; x < description.Size.x; ++x) {
                for (int y = 0; y < description.Size.y; ++y) {
                    NodeDescription nodeDescription = description.GetSurfaceNode(new Vector2Int(x, y));
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

            yield break;

        }


    }

}

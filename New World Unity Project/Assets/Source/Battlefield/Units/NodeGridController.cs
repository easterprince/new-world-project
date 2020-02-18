using System.Collections;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units;
using NewWorld.Utilities.Singletons;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units {

    public class NodeGridController : MonoBehaviour {

        // Fields.

        private NodeController[,] nodes = new NodeController[0, 0];


        // Life cycle.

        private void Start() {
            UnitSystemController.EnsureInstance(this);
            MapController.EnsureInstance(this);
            UnitSystemController.Instance.UnloadedEvent.AddListener(RemoveNodes);
            UnitSystemController.Instance.LoadedEvent.AddListener(PlaceNodes);
        }


        // Support.

        private void RemoveNodes() {
            foreach (var node in nodes) {
                Destroy(node.gameObject);
            }
        }

        private void PlaceNodes() {
            nodes = new NodeController[MapController.Instance.Size.x, MapController.Instance.Size.y];
            foreach (Vector2Int position in Enumerables.InRange2(MapController.Instance.Size)) {
                if (MapController.Instance[position].Type == NodeDescription.NodeType.Abyss) {
                    continue;
                }
                NodeController node = NodeController.BuildNode(position, transform);
                nodes[position.x, position.y] = node;
            }
        }


    }

}

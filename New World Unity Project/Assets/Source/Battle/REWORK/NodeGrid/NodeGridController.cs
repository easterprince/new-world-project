using System.Collections;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Unit;
using NewWorld.Utilities.Singletons;
using NewWorld.Utilities;
using NewWorld.Battlefield.UnitSystem;

namespace NewWorld.Battlefield.NodeGrid {

    public class NodeGridController : MonoBehaviour {

        // Fields.

        private NodeController[,] nodes = new NodeController[0, 0];


        // Life cycle.

        private void Start() {
            UnitSystemController.EnsureInstance(this);
            MapController.EnsureInstance(this);
            UnitSystemController.Instance.UnloadedEvent.AddListener(RemoveNodes);
            UnitSystemController.Instance.LoadedEvent.AddListener(PlaceNodes);
            UnitSystemController.Instance.UnitAddedEvent.AddListener(HighlightNode);
            UnitSystemController.Instance.ConnectedNodeUpdatedEvent.AddListener(HighlightNodeStunt);
            UnitSystemController.Instance.UnitRemovedEvent.AddListener(DarkenNodeStunt);
            UnitSystemController.Instance.ConnectedNodeUpdatedEvent.AddListener(DarkenNodeStunt);
        }

        private void OnDestroy() {
            UnitSystemController.Instance?.UnloadedEvent.RemoveListener(RemoveNodes);
            UnitSystemController.Instance?.LoadedEvent.RemoveListener(PlaceNodes);
            UnitSystemController.Instance?.UnitAddedEvent.RemoveListener(HighlightNode);
            UnitSystemController.Instance?.ConnectedNodeUpdatedEvent.RemoveListener(HighlightNodeStunt);
            UnitSystemController.Instance?.UnitRemovedEvent.RemoveListener(DarkenNodeStunt);
            UnitSystemController.Instance?.ConnectedNodeUpdatedEvent.RemoveListener(DarkenNodeStunt);
        }


        // Support.

        private void RemoveNodes() {
            foreach (var node in nodes) {
                Destroy(node);
            }
        }

        private void PlaceNodes() {
            nodes = new NodeController[MapController.Instance.Size.x, MapController.Instance.Size.y];
            foreach (Vector2Int position in Enumerables.InRange2(MapController.Instance.Size)) {
                NodeController node = NodeController.BuildNode(position, transform);
                if (MapController.Instance[position].Type == NodeDescription.NodeType.Abyss) {
                    node.Shown = false;
                }
                nodes[position.x, position.y] = node;
                DarkenNode(position);
            }
            foreach (UnitController unit in UnitSystemController.Instance) {
                HighlightNode(unit);
            }
        }


        // Event handlers.

        private void HighlightNode(UnitController unit) {
            Vector2Int position = UnitSystemController.Instance.GetConnectedNode(unit);
            nodes[position.x, position.y].Color = Color.white;
        }

        private void HighlightNodeStunt(UnitController unit, Vector2Int position) {
            HighlightNode(unit);
        }

        private void DarkenNode(Vector2Int position) {
            nodes[position.x, position.y].Color = Color.gray;
        }

        private void DarkenNodeStunt(UnitController unit, Vector2Int position) {
            DarkenNode(position);
        }


    }

}

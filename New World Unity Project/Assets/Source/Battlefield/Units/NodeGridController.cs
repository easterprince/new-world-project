﻿using System.Collections;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units;
using NewWorld.Utilities.Singletones;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.Units {

    public class NodeGridController : MonoBehaviour {

        // Fields.

        private NodeController[,] nodes = new NodeController[0, 0];


        // Life cycle.

        private void Start() {
            UnitSystemController.EnsureInstance(this);
            MapController.EnsureInstance(this);
        }

        private void Update() {

            // Adjust array dimensions.
            if (MapController.Instance.Size != new Vector2Int(nodes.GetLength(0), nodes.GetLength(1))) {
                nodes = new NodeController[MapController.Instance.Size.x, MapController.Instance.Size.y];
                foreach (Vector2Int position in Enumerables.InRange2(MapController.Instance.Size)) {
                    NodeController node = NodeController.BuildNode(position);
                    nodes[position.x, position.y] = node;
                }
            }

        }


    }

}

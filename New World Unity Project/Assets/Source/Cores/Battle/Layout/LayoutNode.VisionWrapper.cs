﻿using NewWorld.Utilities;
using NewWorld.Utilities.Graphs;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Cores.Battle.Layout {

    public partial class LayoutNode {

        // Wrapper.

        public class VisionWrapper : ObjectWrapper<LayoutNode>, IWeightedGraphVertex<VisionWrapper, float> {

            // Constructor.

            public VisionWrapper(LayoutNode wrapped) : base(wrapped) {}


            // Properties.

            public Vector2Int Position => Wrapped.Position;

            public IEnumerable<KeyValuePair<VisionWrapper, float>> Adjacency {
                get {
                    foreach (var pair in Wrapped.Visible) {
                        yield return new KeyValuePair<VisionWrapper, float>(pair.Key.AsVision, pair.Value);
                    }
                }
            }


            // Method.

            public float CalculateDistance(VisionWrapper destination) {
                return Wrapped.CalculateDistance(destination.Wrapped);
            }


        }


    }

}

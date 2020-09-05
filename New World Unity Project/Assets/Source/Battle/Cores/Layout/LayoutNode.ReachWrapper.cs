using NewWorld.Utilities;
using NewWorld.Utilities.Graphs;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battle.Cores.Layout {
    
    public partial class LayoutNode {

        // Wrapper.

        public class ReachWrapper : ObjectWrapper<LayoutNode>, IWeightedGraphVertex<ReachWrapper, float> {

            // Constructor.

            public ReachWrapper(LayoutNode wrapped) : base(wrapped) {}


            // Properties.

            public Vector2Int Position => Wrapped.Position;

            public IEnumerable<KeyValuePair<ReachWrapper, float>> Adjacency {
                get {
                    foreach (var pair in Wrapped.Visible) {
                        yield return new KeyValuePair<ReachWrapper, float>(pair.Key.AsReach, pair.Value);
                    }
                }
            }

            
            // Method.

            public float CalculateDistance(ReachWrapper destination) {
                return Wrapped.CalculateDistance(destination.Wrapped);
            }


        }


    }

}

using NewWorld.Utilities.Graphs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NewWorld.Battle.Cores.Layout {

    public partial class LayoutRegion : IWeightedGraphVertex<LayoutRegion, float> {

        // Fields.

        private readonly Vector2Int center;
        private readonly List<LayoutNode> nodes = new List<LayoutNode>();
        private readonly Dictionary<LayoutRegion, float> adjacency = new Dictionary<LayoutRegion, float>();

        // Wrapper.
        private readonly RegionWrapper wrapper;


        // Properties.

        public Vector2Int Center => center;
        public IEnumerable<LayoutNode> Nodes => nodes.AsReadOnly();
        public IReadOnlyDictionary<LayoutRegion, float> Adjacency => new ReadOnlyDictionary<LayoutRegion, float>(adjacency);
        IEnumerable<KeyValuePair<LayoutRegion, float>> IWeightedGraphVertex<LayoutRegion, float>.Adjacency => Adjacency;
        public RegionWrapper Wrapper => wrapper;


        // Constructors.

        public LayoutRegion(Vector2Int center) {
            this.center = center;
            wrapper = new RegionWrapper(this);
        }


        // Methods.

        public float GetDistance(LayoutRegion other) {
            return (center - other.center).magnitude;
        }

        public void AddAdjacent(LayoutRegion adjacent) {
            adjacency[adjacent] = (adjacent.center - center).magnitude;
        }

        public void AddNode(LayoutNode node) {
            nodes.Add(node);
        }


    }

}

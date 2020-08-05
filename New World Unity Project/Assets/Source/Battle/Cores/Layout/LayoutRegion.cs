using NewWorld.Utilities.Graphs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NewWorld.Battle.Cores.Layout {

    public partial class LayoutRegion {

        // Fields.

        private readonly Vector2Int center;
        private readonly Dictionary<LayoutRegion, float> adjacency;

        // Wrapper.
        private readonly RegionWrapper wrapper;


        // Properties.

        public Vector2Int Center => center;

        public IReadOnlyDictionary<LayoutRegion, float> Adjacency => new ReadOnlyDictionary<LayoutRegion, float>(adjacency);

        public RegionWrapper Wrapper => wrapper;


        // Constructors.

        public LayoutRegion(Vector2Int center) {
            this.center = center;
            adjacency = new Dictionary<LayoutRegion, float>();
            wrapper = new RegionWrapper(this);
        }


        // Methods.

        public float GetHeuristic(LayoutRegion destination) {
            return (center - destination.center).magnitude;
        }

        public void AddAdjacent(LayoutRegion adjacent) {
            adjacency[adjacent] = (adjacent.center - center).magnitude;
        }


    }

}

using NewWorld.Utilities;
using NewWorld.Utilities.Graphs;
using System.Collections.Generic;

namespace NewWorld.Battle.Cores.Layout {
    
    public partial class LayoutRegion {

        // Wrapper.

        public class RegionWrapper : ClassWrapper<LayoutRegion>, ILocatedGraphVertex<RegionWrapper> {

            // Constructor.

            public RegionWrapper(LayoutRegion wrapped) : base(wrapped) {}


            // Properties.

            public IEnumerable<KeyValuePair<RegionWrapper, float>> Adjacency {
                get {
                    foreach (var adjacentAndWeight in Wrapped.Adjacency) {
                        yield return new KeyValuePair<RegionWrapper, float>(adjacentAndWeight.Key.Wrapper, adjacentAndWeight.Value);
                    }
                }
            }


            // Methods.

            public float GetHeuristic(RegionWrapper destination) => Wrapped.GetHeuristic(destination.Wrapped);


        }


    }

}

using NewWorld.Utilities;
using NewWorld.Utilities.Graphs;
using System.Collections.Generic;

namespace NewWorld.Cores.Battle.Layout {

    public partial class LayoutRegion {

        // Wrapper.

        public class RegionWrapper : ObjectWrapper<LayoutRegion>, IWeightedGraphVertex<RegionWrapper, float> {

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

            public float GetDistance(RegionWrapper destination) => Wrapped.GetDistance(destination.Wrapped);


        }


    }

}

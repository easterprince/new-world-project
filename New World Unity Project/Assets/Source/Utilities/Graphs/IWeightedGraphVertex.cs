using System.Collections.Generic;

namespace NewWorld.Utilities.Graphs {

    public interface IWeightedGraphVertex<TSelf>
        where TSelf : IWeightedGraphVertex<TSelf> {

        IEnumerable<KeyValuePair<TSelf, float>> Adjacency { get; }


    }

}

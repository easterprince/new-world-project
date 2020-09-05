using System.Collections.Generic;

namespace NewWorld.Utilities.Graphs {

    public interface IWeightedGraphVertex<TSelf, TWeight>
        where TSelf : IWeightedGraphVertex<TSelf, TWeight> {

        IEnumerable<KeyValuePair<TSelf, TWeight>> Adjacency { get; }


    }

}

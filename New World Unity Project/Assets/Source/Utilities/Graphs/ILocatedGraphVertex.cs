namespace NewWorld.Utilities.Graphs {
    
    public interface ILocatedGraphVertex<TSelf> : IWeightedGraphVertex<TSelf>
        where TSelf : ILocatedGraphVertex<TSelf> {

        float GetHeuristic(TSelf destination);


    }

}

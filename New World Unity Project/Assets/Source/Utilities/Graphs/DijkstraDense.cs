using System.Collections.Generic;

namespace NewWorld.Utilities.Graphs {
    
    public class DijkstraDense {

        private class Report<TVertex> {

            public float Traversed { get; set; }
            public TVertex Predecessor { get; set; }
            public bool Finalized { get; set; }

        }


        public static Dictionary<TVertex, float> FindAllReachable<TVertex>(TVertex origin, float distanceLimit)
            where TVertex : class, IWeightedGraphVertex<TVertex, float> {

            var reachable = new Dictionary<TVertex, float>();
            if (origin == null || distanceLimit < 0) {
                return reachable;
            }

            // Initialize collections.
            var reports = new Dictionary<TVertex, Report<TVertex>>();
            var toFinalize = new HashSet<TVertex>();
            var originReport = new Report<TVertex> {
                Traversed = 0,
                Predecessor = null
            };
            reports[origin] = originReport;
            toFinalize.Add(origin);

            // Build partial paths.
            while (toFinalize.Count > 0) {

                // Find free vertex with least cost.
                TVertex currentVertex = null;
                float leastCost = float.PositiveInfinity;
                foreach (var otherVertex in toFinalize) {
                    var otherReport = reports[otherVertex];
                    if (otherReport.Traversed < leastCost) {
                        currentVertex = otherVertex;
                        leastCost = otherReport.Traversed;
                    }
                }
                if (currentVertex == null || leastCost > distanceLimit) {
                    break;
                }
                toFinalize.Remove(currentVertex);

                // Update costs of adjacent vertices.
                var currentReport = reports[currentVertex];
                currentReport.Finalized = true;
                foreach (var vertexAndWeight in currentVertex.Adjacency) {
                    var otherVertex = vertexAndWeight.Key;
                    float traversed = currentReport.Traversed + vertexAndWeight.Value;
                    if (traversed > distanceLimit) {
                        continue;
                    }
                    if (!reports.TryGetValue(otherVertex, out var otherReport)) {
                        otherReport = new Report<TVertex>() {
                            Traversed = traversed,
                            Predecessor = currentVertex
                        };
                        reports[otherVertex] = otherReport;
                        toFinalize.Add(otherVertex);
                        continue;
                    }
                    if (otherReport.Finalized) {
                        continue;
                    }
                    if (traversed < otherReport.Traversed) {
                        otherReport.Traversed = traversed;
                        otherReport.Predecessor = currentVertex;
                    }
                }

            }

            // Return reachable vertices.
            foreach (var vertexAndReport in reports) {
                reachable[vertexAndReport.Key] = vertexAndReport.Value.Traversed;
            }
            return reachable;
        }


    }

}

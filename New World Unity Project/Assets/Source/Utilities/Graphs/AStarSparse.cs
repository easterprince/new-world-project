using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace NewWorld.Utilities.Graphs {
    
    public static class AStarSparse {

        private class Report<TVertex> : IComparable<Report<TVertex>> {

            public TVertex Vertex { get; set; }
            public int Index { get; set; }
            public float Traversed { get; set; }
            public float Heuristic { get; set; }
            public TVertex Predecessor { get; set; }
            public bool Finalized { get; set; }

            public float Cost => Traversed + Heuristic;

            public int CompareTo(Report<TVertex> other) {
                if (Cost != other.Cost) {
                    return Cost < other.Cost ? -1 : 1;
                }
                if (Index != other.Index) {
                    return Index < other.Index ? -1 : 1;
                }
                return 0;
            }

        }


        public static List<TVertex> TryFindShortestPath<TVertex>(TVertex start, TVertex finish)
            where TVertex : class, ILocatedGraphVertex<TVertex> {

            if (start == null || finish == null) {
                return null;
            }

            // Initialize collections.
            int unusedIndex = 0;
            var reports = new Dictionary<TVertex, Report<TVertex>>();
            var toFinalize = new SortedSet<Report<TVertex>>();
            var startReport = new Report<TVertex> {
                Vertex = start,
                Index = unusedIndex++,
                Traversed = 0,
                Heuristic = start.GetHeuristic(finish),
                Predecessor = null
            };
            reports[start] = startReport;
            toFinalize.Add(startReport);

            // Build partial paths.
            while (toFinalize.Count > 0) {

                // Find free vertex with least cost.
                var currentReport = toFinalize.Min;
                if (currentReport.Cost == float.PositiveInfinity) {
                    break;
                }
                var currentVertex = currentReport.Vertex;
                toFinalize.Remove(currentReport);

                // Update costs of adjacent vertices.
                currentReport.Finalized = true;
                if (currentVertex == finish) {
                    break;
                }
                foreach (var vertexAndWeight in currentVertex.Adjacency) {
                    var otherVertex = vertexAndWeight.Key;
                    float traversed = currentReport.Traversed + vertexAndWeight.Value;
                    if (!reports.TryGetValue(otherVertex, out var otherReport)) {
                        otherReport = new Report<TVertex>() {
                            Vertex = otherVertex,
                            Index = unusedIndex++,
                            Traversed = traversed,
                            Heuristic = otherVertex.GetHeuristic(finish),
                            Predecessor = currentVertex
                        };
                        reports[otherVertex] = otherReport;
                        toFinalize.Add(otherReport);
                        continue;
                    }
                    if (otherReport.Finalized) {
                        continue;
                    }
                    if (traversed < otherReport.Traversed) {
                        toFinalize.Remove(otherReport);
                        otherReport.Traversed = traversed;
                        otherReport.Predecessor = currentVertex;
                        toFinalize.Add(otherReport);
                    }
                }

            }

            // Return resulting path.
            if (!reports.ContainsKey(finish)) {
                return null;
            }
            var path = new List<TVertex>();
            var pathVertex = finish;
            path.Add(pathVertex);
            while (pathVertex != null) {
                pathVertex = reports[pathVertex].Predecessor;
                path.Add(pathVertex);
            }
            return path;
        }


    }

}

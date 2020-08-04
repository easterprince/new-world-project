using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NewWorld.Utilities.Graphs {
    
    public static class AStarDense {

        private class Report<TVertex> {

            public float Traversed { get; set; }
            public float Heuristic { get; set; }
            public TVertex Predecessor { get; set; }
            public bool Finalized { get; set; }

            public float Cost => Traversed + Heuristic;

        }


        public static List<TVertex> TryFindShortestPath<TVertex>(TVertex start, TVertex finish)
            where TVertex : class, ILocatedGraphVertex<TVertex> {

            if (start == null || finish == null) {
                return null;
            }

            // Initialize collections.
            var reports = new Dictionary<TVertex, Report<TVertex>>();
            var toFinalize = new HashSet<TVertex>();
            var startReport = new Report<TVertex> {
                Traversed = 0,
                Heuristic = start.GetHeuristic(finish),
                Predecessor = null
            };
            reports[start] = startReport;
            toFinalize.Add(start);

            // Build partial paths.
            while (toFinalize.Count > 0) {

                // Find free vertex with least cost.
                TVertex currentVertex = null;
                float leastCost = float.PositiveInfinity;
                foreach (var otherVertex in toFinalize) {
                    var otherReport = reports[otherVertex];
                    if (otherReport.Cost < leastCost) {
                        currentVertex = otherVertex;
                        leastCost = otherReport.Cost;
                    }
                }
                if (currentVertex == null) {
                    break;
                }
                toFinalize.Remove(currentVertex);

                // Update costs of adjacent vertices.
                var currentReport = reports[currentVertex];
                currentReport.Finalized = true;
                if (currentVertex == finish) {
                    break;
                }
                foreach (var vertexAndWeight in currentVertex.Adjacency) {
                    var otherVertex = vertexAndWeight.Key;
                    float traversed = currentReport.Traversed + vertexAndWeight.Value;
                    if (!reports.TryGetValue(otherVertex, out var otherReport)) {
                        otherReport = new Report<TVertex>() {
                            Traversed = traversed,
                            Heuristic = otherVertex.GetHeuristic(finish),
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

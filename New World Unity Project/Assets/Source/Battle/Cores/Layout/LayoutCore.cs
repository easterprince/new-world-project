using NewWorld.Battle.Cores.Battlefield;
using NewWorld.Battle.Cores.Map;
using NewWorld.Utilities;
using NewWorld.Utilities.Graphs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Battle.Cores.Layout {

    public class LayoutCore : ConnectableCoreBase<LayoutCore, LayoutPresentation, BattlefieldPresentation> {

        // Static.

        public static async Task<LayoutCore> CreateLayoutAsync(
            MapPresentation map, float regionRadius, float bodyRadius, float toleranceRadius, CancellationToken cancellation) {
            
            if (map == null) {
                throw new ArgumentNullException(nameof(map));
            }

            var layout = new LayoutCore();
            await layout.ReadNodes(map, cancellation);
            await layout.BuildNodeGraph(regionRadius, bodyRadius, toleranceRadius, cancellation);
            //await layout.BuildRegions(cancellation);
            return layout;
        }


        // Fields.

        private LayoutNode[,] nodes;
        private List<LayoutRegion> regions;
        private float regionRadius = 0;
        private float bodyRadius = 0;


        // Constructors.

        public LayoutCore() {
            nodes = new LayoutNode[0, 0];
            regions = new List<LayoutRegion>();
        }

        public LayoutCore(LayoutCore other) {

            // Copy regions.
            regions = new List<LayoutRegion>();
            var oldToNew = new Dictionary<LayoutRegion, LayoutRegion>();
            foreach (var oldRegion in other.regions) {
                var newRegion = new LayoutRegion(oldRegion.Center);
                regions.Add(newRegion);
                oldToNew[oldRegion] = newRegion;
            }
            foreach (var oldRegion in regions) {
                var newRegion = oldToNew[oldRegion];
                foreach (var pair in oldRegion.Adjacency) {
                    var oldAdjacent = pair.Key;
                    var newAdjacent = oldToNew[oldAdjacent];
                    newRegion.AddAdjacent(newAdjacent);
                }
            }

            // Copy nodes.
            nodes = new LayoutNode[other.NodeCount.x, other.NodeCount.y];
            foreach (var position in Enumerables.InRange2(NodeCount)) {
                var oldNode = other.nodes[position.x, position.y];
                if (oldNode is null) {
                    continue;
                }
                nodes[position.x, position.y] = new LayoutNode(position);
            }
            foreach (var position in Enumerables.InRange2(NodeCount)) {
                var oldNode = other.nodes[position.x, position.y];
                if (oldNode is null) {
                    continue;
                }
                var newNode = nodes[position.x, position.y];
                if (!(oldNode.Region is null)) {
                    newNode.Region = oldToNew[oldNode.Region];
                    newNode.Region.AddNode(newNode);
                }
                foreach (var pair in oldNode.Reachable) {
                    var oldReachable = pair.Key;
                    var newReachable = nodes[oldReachable.Position.x, oldReachable.Position.y];
                    if (oldNode.Visible.ContainsKey(oldReachable)) {
                        newNode.AddVisible(newReachable);
                    } else {
                        var distance = pair.Value;
                        newNode.AddReachable(newReachable, distance);
                    }
                }
            }

        }


        // Properties.

        public Vector2Int NodeCount => new Vector2Int(nodes.GetLength(0), nodes.GetLength(1));
        public float RegionRadius => regionRadius;
        public float BodyRadius => bodyRadius;


        // Cloning.

        public override LayoutCore Clone() {
            return new LayoutCore(this);
        }


        // Presentation generation.

        private protected override LayoutPresentation BuildPresentation() {
            return new LayoutPresentation(this);
        }


        // Building methods.

        private async Task ReadNodes(MapPresentation map, CancellationToken cancellation) {
            if (map is null) {
                throw new ArgumentNullException(nameof(map));
            }
            cancellation.ThrowIfCancellationRequested();

            // Place nodes.
            await Task.Run(() => {
                nodes = new LayoutNode[map.Size.x, map.Size.y];
                foreach (var position in Enumerables.InRange2(NodeCount)) {
                    if (map[position].Type != MapNode.NodeType.Common) {
                        continue;
                    }
                    nodes[position.x, position.y] = new LayoutNode(position);
                }
                cancellation.ThrowIfCancellationRequested();
            });

        }

        private async Task BuildNodeGraph(float regionRadius, float bodyRadius, float toleranceRadius, CancellationToken cancellation) {
            this.regionRadius = Mathf.Max(regionRadius, 0);
            this.bodyRadius = Mathf.Max(bodyRadius, 0);
            cancellation.ThrowIfCancellationRequested();

            // Build stencil.
            var stencil = await VisionStencil.BuildAsync(regionRadius, bodyRadius, toleranceRadius);
            cancellation.ThrowIfCancellationRequested();

            // Build vision edges. 
            int visionMaxOffset = Mathf.FloorToInt(regionRadius);
            var tasks = new List<Task>();
            foreach (var node in nodes) {
                if (node is null) {
                    continue;
                }
                var currentNode = node;
                var task = Task.Run(() => {

                    // See all nodes around current one; add edge in graph for every visible node. 
                    foreach (var destinationPosition in Enumerables.InSegment2(
                        currentNode.Position - visionMaxOffset * Vector2Int.one, currentNode.Position + visionMaxOffset * Vector2Int.one)) {

                        cancellation.ThrowIfCancellationRequested();
                        if (!Enumerables.IsIndex(destinationPosition, nodes)) {
                            continue;
                        }
                        var visionPath = stencil.GetVisionPath(currentNode.Position, destinationPosition);
                        if (visionPath is null) {
                            continue;
                        }
                        bool visible = true;
                        foreach (var passedPosition in visionPath) {
                            var passedNode = nodes[passedPosition.x, passedPosition.y];
                            if (passedNode is null) {
                                visible = false;
                                break;
                            }
                        }
                        if (visible) {
                            var destinationNode = nodes[destinationPosition.x, destinationPosition.y];
                            currentNode.AddVisible(destinationNode);
                        }

                    }

                });
                tasks.Add(task);
                cancellation.ThrowIfCancellationRequested();
            }
            await Task.WhenAll(tasks);
            cancellation.ThrowIfCancellationRequested();

            // Build reach edges.
            tasks.Clear();
            foreach (var node in nodes) {
                if (node is null) {
                    continue;
                }
                var currentNode = node;
                var task = Task.Run(() => {

                    // Try reach all nodes around current one; add edge in graph for every reachable node. 
                    var reachable = DijkstraDense.FindAllReachable(currentNode.AsVision, regionRadius);
                    cancellation.ThrowIfCancellationRequested();
                    foreach (var nodeAndDistance in reachable) {
                        var reachedPosition = nodeAndDistance.Key.Position;
                        var reachedNode = nodes[reachedPosition.x, reachedPosition.y];
                        currentNode.AddReachable(reachedNode, nodeAndDistance.Value);
                    }
                    cancellation.ThrowIfCancellationRequested();

                });
                tasks.Add(task);
                cancellation.ThrowIfCancellationRequested();
            }
            await Task.WhenAll(tasks);
            cancellation.ThrowIfCancellationRequested();

        }

        private async Task BuildRegions(CancellationToken cancellation) {
            throw new NotImplementedException();
        }


        // Information methods.

        public List<Vector2Int> TryFindShortestPath(Vector2Int originPosition, Vector2Int destinationPosition) {
            if (!Enumerables.IsIndex(originPosition, nodes) || !Enumerables.IsIndex(destinationPosition, nodes)) {
                return null;
            }
            var originNode = nodes[originPosition.x, originPosition.y];
            var destinationNode = nodes[destinationPosition.x, destinationPosition.y];
            if (originNode is null || destinationNode is null) {
                return null;
            }
            var nodePath = AStarSparse.TryFindShortestPath(
                originNode.AsVision, destinationNode.AsVision, destinationNode.AsVision.CalculateDistance);
            var path = new List<Vector2Int>(nodePath.ConvertAll((node) => node.Position));
            return path;
        }



    }

}

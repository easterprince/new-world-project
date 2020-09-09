using NewWorld.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Cores.Battle.Layout {

    public struct VisionStencil {

        // Fabric.

        public async static Task<VisionStencil> BuildAsync(float visionRadius, float bodyRadius, float toleranceRadius) {
            var stencil = await Task.Run(() => new VisionStencil(visionRadius, bodyRadius, toleranceRadius));
            return stencil;
        }


        // Fields.

        private readonly float visionRadius;
        private readonly float bodyRadius;
        private readonly float toleranceRadius;
        private readonly List<Vector2Int> possiblyVisible;
        private readonly List<Vector2Int>[,] visionPaths;


        // Constructor.

        public VisionStencil(float visionRadius, float bodyRadius, float toleranceRadius) {

            // Initialize fields.
            this.visionRadius = Mathf.Max(visionRadius, 0);
            this.bodyRadius = Mathf.Clamp(bodyRadius, 0, 0.5f);
            this.toleranceRadius = Mathf.Clamp(toleranceRadius, 0, 0.49f);
            possiblyVisible = new List<Vector2Int>();
            int arrayDimension = Mathf.FloorToInt(this.visionRadius) + 1;
            visionPaths = new List<Vector2Int>[arrayDimension, arrayDimension];

            // Calculate stencil.
            foreach (var position in Enumerables.Index(visionPaths)) {
                TrackVision(position);
            }

            /*WriteDebugInFile();*/

        }


        // Properties.

        public float VisionRadius => visionRadius;
        public float BodyRadius => bodyRadius;
        public float ToleranceRadius => toleranceRadius;


        // Informational methods.

        public IEnumerable<Vector2Int> GetVisionPath(Vector2Int origin, Vector2Int destination) {
            Vector2Int difference = destination - origin;
            Vector2Int stencilDifference = new Vector2Int(Mathf.Abs(difference.x), Mathf.Abs(difference.y));
            if (!Enumerables.IsIndex(stencilDifference, visionPaths)) {
                return null;
            }
            var visionPath = visionPaths[stencilDifference.x, stencilDifference.y];
            if (visionPath == null) {
                return null;
            }

            IEnumerable<Vector2Int> Iterate() {
                foreach (var localPosition in visionPath) {
                    var position = localPosition;
                    position.x *= (int) Mathf.Sign(difference.x);
                    position.y *= (int) Mathf.Sign(difference.y);
                    position += origin;
                    yield return position;
                }
            };

            return Iterate();
        }


        // Calculation method.

        // Collect nodes on vision path to destination position.
        // Attention! If body radius is less than tolerance radius,
        // vision path may jump over nodes (indifferently if they are passable or not).
        private void TrackVision(in Vector2Int destination) {

            // Check if position may be visible at all.
            if (destination.magnitude > visionRadius) {
                return;
            }
            possiblyVisible.Add(destination);
            var visionPath = new List<Vector2Int>();
            visionPaths[destination.x, destination.y] = visionPath;
            if (destination == Vector2Int.zero) {
                visionPath.Add(destination);
                return;
            }

            // Collect positions that must passable for this position to be reachable.
            float pathDistance = destination.magnitude;
            foreach (var passedPosition in Enumerables.InSegment2(destination)) {

                // Get corners of non-tolerant area of node.
                float cornerShift = 0.5f - toleranceRadius;
                var corners = new Vector2[] {
                    passedPosition + cornerShift * new Vector2(1, 1),
                    passedPosition + cornerShift * new Vector2(1, -1),
                    passedPosition + cornerShift * new Vector2(-1, 1),
                    passedPosition + cornerShift * new Vector2(-1, -1)
                };

                // Calculate distances from path to corners.
                // If at least two corners have distances of different signs,
                // it means that path goes between them, i.e. through non-tolerant area.
                bool passed = false;
                var distances = new float[corners.Length];
                int lastNonZeroSign = 0;
                for (int i = 0; i < corners.Length; ++i) {
                    distances[i] = (destination.y * corners[i].x - destination.x * corners[i].y) / pathDistance;
                    if (distances[i] != 0) {
                        int sign = (int) Mathf.Sign(distances[i]);
                        if (lastNonZeroSign == 0) {
                            lastNonZeroSign = sign;
                        } else if (lastNonZeroSign != sign) {
                            passed = true;
                            break;
                        }
                    }
                }

                // Even when path itself doesn't intersect with non-tolerant area,
                // the body itself may collide with this area,
                // if distance from line to corners is small enough.
                if (!passed) {
                    foreach (float distance in distances) {
                        if (Mathf.Abs(distance) < bodyRadius) {
                            passed = true;
                            break;
                        }
                    }
                }

                // Add position if it is passed.
                if (passed) {
                    visionPath.Add(passedPosition);
                }

            }

        }


        // Debug.

        private static int written = 0;

        private void WriteDebugInFile() {
            int index = ++written;
            var output = File.CreateText($"stencil{index}.txt");
            output.WriteLine("STENCIL");
            output.WriteLine($"visionRadius = {visionRadius}");
            output.WriteLine($"bodyRadius = {bodyRadius}");
            output.WriteLine($"toleranceRadius = {toleranceRadius}");
            output.WriteLine();
            foreach (var destination in Enumerables.Index(visionPaths)) {
                var path = visionPaths[destination.x, destination.y];
                if (path != null) {
                    var passed = new bool[destination.x + 1, destination.y + 1];
                    foreach (var position in path) {
                        passed[position.x, position.y] = true;
                    }
                    output.WriteLine($"{destination}");
                    foreach (var position in Enumerables.Index(passed)) {
                        output.Write(passed[position.x, position.y] ? "1" : "0");
                        if (position.y == destination.y) {
                            output.WriteLine();
                        }
                    }
                    output.WriteLine();
                }
            }
            output.Close();
        }


    }

}

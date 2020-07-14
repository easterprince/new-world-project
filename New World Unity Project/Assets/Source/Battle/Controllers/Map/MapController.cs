using NewWorld.Battle.Cores.Map;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Map {
    
    public class MapController : BuildableController {

        // Fields.

        // Presentation.
        private MapPresentation presentation;

        // Unity references.
        [SerializeField]
        private GameObject clustersObject;
        private ClusterController[,] clusters = new ClusterController[0, 0];

        // Parameters.
        [SerializeField]
        [Range(0, 10)]
        private int edgeWidth = 0;


        // Properties.

        public MapPresentation Presentation {
            get => presentation;
        }

        public GameObject ClustersObject {
            get => clustersObject;
            set => clustersObject = value;
        }

        public int EdgeWidth {
            get => edgeWidth;
            set => edgeWidth = value;
        }


        // Building.

        public void StartBuilding(MapPresentation presentation) {
            if (presentation == null) {
                throw new ArgumentNullException(nameof(presentation));
            }
            if (this.presentation != null) {
                throw new InvalidOperationException("Presentation has been already set!");
            }
            this.presentation = presentation;

            // Check if there's nothing to build.
            if (clustersObject == null || edgeWidth == 0 && presentation.Size == Vector2Int.zero) {
                Built = true;
                return;
            }

            // Plan clusters.
            var firstCluster = ClusterController.MakeEmptyCluster();
            var clusterSize = firstCluster.Size;
            var clusterCount = presentation.Size + 2 * new Vector2Int(edgeWidth, edgeWidth);
            clusterCount.x = (clusterCount.x + clusterSize.x - 1) / clusterSize.x;
            clusterCount.y = (clusterCount.y + clusterSize.y - 1) / clusterSize.y;
            clusters = new ClusterController[clusterCount.x, clusterCount.y];

            // Start building clusters.
            int builtCount = 0; // Will be captured by cluster onBuilt handlers.
            foreach (var clusterIndex in Enumerables.InRange2(clusterCount)) {
                var startingPosition = clusterIndex * clusterSize - new Vector2Int(edgeWidth, edgeWidth);
                ClusterController cluster;
                if (clusterIndex == Vector2Int.zero) {
                    cluster = firstCluster;
                    cluster.StartBuilding(presentation, startingPosition);
                } else {
                    cluster = ClusterController.StartBuildingCluster(presentation, startingPosition);
                }
                cluster.transform.parent = clustersObject.transform;
                cluster.ExecuteWhenBuilt(this, () => {
                    ++builtCount;
                    if (builtCount == clusters.Length) {
                        Built = true;
                    }
                });
            }

        }


    }

}

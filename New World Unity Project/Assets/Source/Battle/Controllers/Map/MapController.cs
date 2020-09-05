using NewWorld.Battle.Cores.Map;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Map {
    
    public class MapController : BuildableController {

        // Fields.

        private MapPresentation presentation = null;
        private ClusterController[,] clusters = new ClusterController[0, 0];

        // Steady references.
        [SerializeField]
        private GameObject clustersObject;

        // Building parameters.
        [SerializeField]
        [Range(0, 10)]
        private int edgeWidth = 0;


        // Properties.

        public MapPresentation Presentation {
            get => presentation;
        }

        public GameObject ClustersObject {
            get => clustersObject;
            set {
                ValidateBeingNotStarted();
                clustersObject = value;
            }
        }

        public int EdgeWidth {
            get => edgeWidth;
            set {
                ValidateNotStartedBuilding();
                edgeWidth = Mathf.Max(0, value);
            }
        }


        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();
            GameObjects.ValidateReference(clustersObject, nameof(clustersObject));
        }


        // Building.

        public void StartBuilding(MapPresentation presentation) {
            if (presentation == null) {
                throw new ArgumentNullException(nameof(presentation));
            }
            ValidateBeingStarted();
            ValidateNotStartedBuilding();

            // Set fields.
            SetStartedBuilding();
            this.presentation = presentation;

            // Check if there's anything to build at all.
            if (edgeWidth == 0 && presentation.Size == Vector2Int.zero) {
                SetFinishedBuilding();
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
                        SetFinishedBuilding();
                    }
                });
            }

        }


    }

}

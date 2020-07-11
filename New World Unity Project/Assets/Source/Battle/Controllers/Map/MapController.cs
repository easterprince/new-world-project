using NewWorld.Battle.Cores.Map;
using NewWorld.Utilities;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Map {
    
    public class MapController : MonoBehaviour {

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
            set {
                presentation = value;
                Rebuild();
            }
        }

        public int EdgeWidth {
            get => edgeWidth;
            set {
                edgeWidth = value;
                Rebuild();
            }
        }

        public GameObject ClustersObject {
            get => clustersObject;
            set => clustersObject = value;
        }


        // Building.

        private void Rebuild() {

            // Clearing.
            foreach (var controller in clusters) {
                Destroy(controller.gameObject);
            }
            clusters = new ClusterController[0, 0];

            // Building.
            if (presentation == null || clustersObject == null) {
                return;
            }
            if (edgeWidth == 0 && presentation.Size == Vector2Int.zero) {
                return;
            }
            var firstCluster = ClusterController.BuildCluster();
            var clusterSize = firstCluster.Size;
            var clusterCount = presentation.Size + 2 * new Vector2Int(edgeWidth, edgeWidth);
            clusterCount.x = (clusterCount.x + clusterSize.x - 1) / clusterSize.x;
            clusterCount.y = (clusterCount.y + clusterSize.y - 1) / clusterSize.y;
            clusters = new ClusterController[clusterCount.x, clusterCount.y];
            foreach (var clusterIndex in Enumerables.InRange2(clusterCount)) {
                var startingPosition = clusterIndex * clusterSize - new Vector2Int(edgeWidth, edgeWidth);
                ClusterController cluster;
                if (clusterIndex == Vector2Int.zero) {
                    cluster = firstCluster;
                    cluster.StartingPosition = startingPosition;
                    cluster.Presentation = presentation;
                } else {
                    cluster = ClusterController.BuildCluster(presentation, startingPosition);
                }
                cluster.gameObject.transform.parent = clustersObject.transform;
            }

        }


    }

}

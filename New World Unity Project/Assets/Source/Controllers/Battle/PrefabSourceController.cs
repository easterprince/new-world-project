using NewWorld.Utilities.Singletons;
using UnityEngine;

namespace NewWorld.Controllers.Battle {

    public class PrefabSourceController : ControllerSingletonBase<PrefabSourceController> {

        // Fields.

        [SerializeField]
        private GameObject unitPrefab;
        [SerializeField]
        private GameObject nodePrefab;
        [SerializeField]
        private GameObject clusterPrefab;


        // Properties.

        public GameObject UnitPrefab {
            get => unitPrefab;
            set => unitPrefab = value;
        }

        public GameObject NodePrefab {
            get => nodePrefab;
            set => nodePrefab = value;
        }

        public GameObject ClusterPrefab {
            get => clusterPrefab;
            set => clusterPrefab = value;
        }


    }

}

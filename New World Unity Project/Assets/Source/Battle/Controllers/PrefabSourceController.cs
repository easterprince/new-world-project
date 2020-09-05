using NewWorld.Utilities.Singletons;
using UnityEngine;

namespace NewWorld.Battle.Controllers {
    
    public class PrefabSourceController : SceneSingleton<PrefabSourceController> {

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

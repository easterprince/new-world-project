using NewWorld.Utilities.Singletons;
using UnityEngine;

namespace NewWorld.Battle.Controllers {
    
    public class PrefabSourceController : SceneSingleton<PrefabSourceController> {

        // Fields.
        
        [SerializeField]
        private GameObject unitPrefab;
        [SerializeField]
        private GameObject nodePrefab;


        // Properties.

        public GameObject UnitPrefab {
            get => unitPrefab;
            set => unitPrefab = value;
        }

        public GameObject NodePrefab {
            get => nodePrefab;
            set => nodePrefab = value;
        }


    }

}

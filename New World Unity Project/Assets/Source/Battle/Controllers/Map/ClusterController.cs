using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Map {
    
    public class ClusterController : MonoBehaviour {

        // Fields.

        private Terrain terrain;


        // Properties.

        public Vector2Int Size => new Vector2Int(
            Mathf.FloorToInt(terrain.terrainData.size.x),
            Mathf.FloorToInt(terrain.terrainData.size.z));


        // Life cycle.

        private void Awake() {
            terrain = GetComponent<Terrain>();
            GameObjects.ValidateComponent(terrain);
        }


    }

}

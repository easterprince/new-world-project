using UnityEngine;

namespace NewWorld.Battle.Cores.Map {

    public class MapPresentation : PresentationBase<MapCore> {
        
        // Constructor.
        
        public MapPresentation(MapCore presented) : base(presented) { }


        // Properties.

        public Vector2Int Size => Presented.Size;
        public float HeightLimit => Presented.HeightLimit;
        public MapNode this[Vector2Int position] => Presented[position];


        // Methods.

        public bool IsRealPosition(in Vector2Int position) => Presented.IsRealPosition(position);
        public Vector2Int GetNearestRealPosition(Vector2Int position) => Presented.GetNearestRealPosition(position);


    }

}

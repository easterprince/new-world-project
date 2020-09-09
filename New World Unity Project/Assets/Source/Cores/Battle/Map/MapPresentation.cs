using UnityEngine;

namespace NewWorld.Cores.Battle.Map {

    public class MapPresentation : PresentationBase<MapCore> {

        // Constructor.

        public MapPresentation(MapCore presented) : base(presented) {}


        // Properties.

        public Vector2Int Size => Presented.Size;
        public float HeightLimit => Presented.HeightLimit;
        public MapNode this[Vector2Int position] => Presented[position];


        // Methods.

        public Vector2Int GetNearestPosition(Vector3 point) => Presented.GetNearestPosition(point);
        public Vector2Int GetNearestRealPosition(Vector3 point) => Presented.GetNearestRealPosition(point);
        public bool IsRealPosition(in Vector2Int position) => Presented.IsRealPosition(position);
        public Vector2Int GetNearestRealPosition(Vector2Int position) => Presented.GetNearestRealPosition(position);
        public float GetHeight(Vector3 point) => Presented.GetHeight(point);


    }

}

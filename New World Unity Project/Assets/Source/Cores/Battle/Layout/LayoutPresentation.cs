using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Cores.Battle.Layout {

    public class LayoutPresentation : PresentationBase<LayoutCore> {

        // Constructor.

        public LayoutPresentation(LayoutCore presented) : base(presented) {}


        // Informational methods.

        public List<Vector2Int> TryFindShortestPath(Vector2Int origin, Vector2Int destination) =>
            Presented.TryFindShortestPath(origin, destination);


    }

}
using NewWorld.Utilities.Graphs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NewWorld.Battle.Cores.Layout {
    
    public partial class LayoutNode {

        // Fields.

        private readonly Vector2Int position;
        private LayoutRegion region = null;
        private readonly Dictionary<LayoutNode, float> visible = new Dictionary<LayoutNode, float>();
        private readonly Dictionary<LayoutNode, float> reachable = new Dictionary<LayoutNode, float>();

        // Wrappers.
        private readonly VisionWrapper vision;
        private readonly ReachWrapper reach;


        // Properties.

        public Vector2Int Position => position;

        public LayoutRegion Region {
            get => region;
            set => region = value;
        }

        public IReadOnlyDictionary<LayoutNode, float> Visible => new ReadOnlyDictionary<LayoutNode, float>(visible);
        public IReadOnlyDictionary<LayoutNode, float> Reachable => new ReadOnlyDictionary<LayoutNode, float>(reachable);

        public VisionWrapper AsVision => vision;
        public ReachWrapper AsReach => reach;


        // Constructors.

        public LayoutNode(Vector2Int position) {
            this.position = position;
            vision = new VisionWrapper(this);
            reach = new ReachWrapper(this);
        }


        // Methods.

        public float CalculateDistance(LayoutNode other) {
            return (position - other.position).magnitude;
        }

        public void AddVisible(LayoutNode other) {
            var distance = CalculateDistance(other);
            visible[other] = distance;
        }

        public void AddReachable(LayoutNode other, float distance) {
            reachable[other] = distance;
        }


    }

}

using UnityEngine;

namespace NewWorld.Cores.Battle.Map {
    
    public struct MapNode {

        // Enumerator.

        public enum NodeType {
            Abyss = 0,
            Common
        }


        // Fields.

        private NodeType type;
        private float height;


        // Constructors.

        public MapNode(NodeType type = NodeType.Abyss, float height = 0) {
            this.type = type;
            this.height = height;
        }


        // Properties.

        public NodeType Type {
            get => type;
            set => type = value;
        }

        public float Height {
            get {
                if (type == NodeType.Abyss) {
                    return float.NegativeInfinity;
                }
                return height;
            }
            set => height = Mathf.Max(value, 0);
        }


    }

}

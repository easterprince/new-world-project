﻿using UnityEngine;

namespace NewWorld.Battlefield.Map {

    public struct NodeDescription {

        // Enumerator.

        public enum NodeType {
            Abyss = 0,
            Common
        }


        // Fields.

        private NodeType type;
        private float height;


        // Properties.

        public NodeType Type {
            get => type;
            set => type = value;
        }

        public float Height {
            get => height;
            set => height = Mathf.Max(value, 0);
        }


        // Constructors.

        public NodeDescription(NodeType type = NodeType.Abyss, float height = 0) {
            this.type = type;
            this.height = height;
        }


    }

}
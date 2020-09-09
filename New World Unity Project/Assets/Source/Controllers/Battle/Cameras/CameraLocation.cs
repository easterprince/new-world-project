using UnityEngine;

namespace NewWorld.Controllers.Battle.Cameras {

    public struct CameraLocation {

        // Fields.

        private Vector3 viewedPosition;
        private float viewingDistance;
        private Quaternion rotation;


        // Properties.

        public Vector3 ViewedPosition {
            get => viewedPosition;
            set => viewedPosition = value;
        }

        public float ViewingDistance {
            get => viewingDistance;
            set => viewingDistance = value;
        }

        public Quaternion Rotation {
            get => rotation;
            set => rotation = value;
        }


    }

}

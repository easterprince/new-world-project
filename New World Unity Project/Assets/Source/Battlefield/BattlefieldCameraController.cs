using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Utilities.Singletons;
using NewWorld.Battlefield.Map;

namespace NewWorld.Battlefield {

    public class BattlefieldCameraController : SceneSingleton<BattlefieldCameraController> {

        // Constants.

        private const float viewingDistanceLowerLimit = 1;
        private const float viewingDistanceUpperLimit = 100;


        // Fields.

        // GameObject components.
        private Camera cameraComponent;

        // Current location.
        private Vector2 currentViewedPosition;
        private float currentViewingDistance;
        private Quaternion currentRotation;

        // Default location.
        private Vector2 defaultViewedPosition;
        private float defaultViewingDistance;
        private Quaternion defaultRotation;

#pragma warning disable IDE0044, CS0414, CS0649

        [Header("Motion and rotation speed")]
        [SerializeField][Range(0.0f, 100.0f)]
        private float motionSpeed = 1.0f;
        [SerializeField][Range(0.0f, 100.0f)]
        private float rotationSpeed = 1.0f;
        [SerializeField][Range(0.0f, 100.0f)]
        private float zoomingSpeed = 1.0f;

        [Header("Height")]
        [SerializeField][Range(viewingDistanceLowerLimit, viewingDistanceUpperLimit)]
        private float minViewingDistance = 10.0f;
        [SerializeField][Range(viewingDistanceLowerLimit, viewingDistanceUpperLimit)]
        private float maxViewingDistance = 20.0f;

#pragma warning restore IDE0044, CS0414, CS0649

        private void OnValidate() {

            // Validate speed parameters.
            motionSpeed = Mathf.Max(motionSpeed, 0f);
            rotationSpeed = Mathf.Max(rotationSpeed, 0f);
            zoomingSpeed = Mathf.Max(zoomingSpeed, 0f);

            // Validate height parameters.
            minViewingDistance = Mathf.Clamp(minViewingDistance, viewingDistanceLowerLimit, viewingDistanceUpperLimit);
            maxViewingDistance = Mathf.Clamp(maxViewingDistance, viewingDistanceLowerLimit, viewingDistanceUpperLimit);

        }


        // Properties.

        public Camera CameraComponent => cameraComponent;


        // Life cycle.

        override private protected void Awake() {
            base.Awake();
        }

        private void Start() {
            BattlefieldController.EnsureInstance(this);
            MapController.EnsureInstance(this);
            cameraComponent = GetComponent<Camera>() ?? throw new MissingComponentException("Missing Camera component!");
        }

        private void Update() {

            /*
            // Correct current location.
            float deltaTime = Time.unscaledDeltaTime;
            if (BattlefieldController.Instance.BattleStarted) {

                // Input processing.
                if (Input.GetAxisRaw("Cancel") == 0) {
                
                    Vector3 positionAddition = Vector3.zero;
                    positionAddition += transform.right * Input.GetAxisRaw("Common X");
                    positionAddition += transform.forward * Input.GetAxisRaw("Common Y");
                    //positionAddition *= motionSpeedModifier;
                    transform.position += positionAddition;
                    //transform.rotation *= Quaternion.Euler(0, Input.GetAxisRaw("Turn") * rotationSpeedModifier, 0);
                    //float newSize = cameraComponent.orthographicSize + scrollingSpeedModifier * -Input.GetAxisRaw("Common Z");
                    //cameraComponent.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);
                
                } else {

                    currentPosition = defaultPosition;
                    currentRotation = defaultRotation;
                    //cameraComponent.orthographicSize = defaultSize;
                
                }

            }

            // Apply current location.
            */
        }


        // TODO. Make method (not properties) to change position/rotation/distance.


        // Support.

        private void CorrectViewedPosition(ref Vector2 position) {
            Vector2Int mapSize = MapController.Instance.Size;
            position.x = Mathf.Clamp(position.x, -1, mapSize.x);
            position.y = Mathf.Clamp(position.y, -1, mapSize.y);
        }

        private void CorrectViewingDistance(ref float distance) {
            distance = Mathf.Clamp(distance, minViewingDistance, maxViewingDistance);
        }


    }

}

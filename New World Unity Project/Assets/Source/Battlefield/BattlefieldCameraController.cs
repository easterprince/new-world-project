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

        [SerializeField]
        private GameObject cameraHolder;

        [Header("Motion and rotation speed")]
        [SerializeField][Range(1f, 100.0f)]
        private float motionSpeed = 10.0f;
        [SerializeField][Range(10f, 1000.0f)]
        private float zoomingSpeed = 100.0f;
        [SerializeField][Range(0.01f, 1.0f)]
        private float rotationSpeed = 0.1f;

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
            defaultViewedPosition = Vector2.zero;
            defaultViewingDistance = maxViewingDistance;
            defaultRotation = transform.rotation;
            currentViewedPosition = defaultViewedPosition;
            currentViewingDistance = defaultViewingDistance;
            currentRotation = defaultRotation;
        }

        private void Start() {
            BattlefieldController.EnsureInstance(this);
            MapController.EnsureInstance(this);
            if (cameraHolder == null) {
                throw new MissingReferenceException("Missing Camera Holder GameObject!");
            }
            cameraComponent = cameraHolder.GetComponent<Camera>();
            if (cameraComponent == null) {
                throw new MissingComponentException("Camera Holder is missing Camera component!");
            }
        }

        private void Update() {

            // Update current location.
            float deltaTime = Time.unscaledDeltaTime;
            if (BattlefieldController.Instance.BattleStarted) {

                // Process input.
                if (Input.GetAxisRaw("Cancel") == 0) {
                    
                    // Update viewed position.
                    Vector3 positionChange = Vector3.zero;
                    positionChange += transform.right * Input.GetAxisRaw("Common X");
                    positionChange += transform.forward * Input.GetAxisRaw("Common Y");
                    positionChange *= motionSpeed * deltaTime;
                    currentViewedPosition += new Vector2(positionChange.x, positionChange.z);

                    // Update distance.
                    float distanceChange = zoomingSpeed * deltaTime * -Input.GetAxisRaw("Common Z");
                    currentViewingDistance += distanceChange;

                    // Update rotation.
                    Quaternion additionalRotation = Quaternion.AngleAxis(360 * rotationSpeed * deltaTime * Input.GetAxisRaw("Turn"), transform.up);
                    currentRotation *= additionalRotation;

                } else {

                    // Reset location.
                    currentViewedPosition = defaultViewedPosition;
                    currentViewingDistance = defaultViewingDistance;
                    currentRotation = defaultRotation;

                }

            }
            CorrectViewedPosition(ref currentViewedPosition);
            CorrectViewingDistance(ref currentViewingDistance);

            // Apply current location.
            transform.rotation = currentRotation;
            float originY = Mathf.Max(MapController.Instance.GetSurfaceHeight(currentViewedPosition), 0);
            Vector3 origin = new Vector3(currentViewedPosition.x, originY, currentViewedPosition.y);
            Vector3 offset = currentViewingDistance * -cameraHolder.transform.forward;
            transform.position = origin + offset;

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

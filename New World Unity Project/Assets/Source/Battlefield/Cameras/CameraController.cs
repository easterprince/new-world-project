using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Utilities.Singletons;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units;

namespace NewWorld.Battlefield.Cameras {

    public class CameraController : MonoBehaviour {

        // Constants.

        private const float viewingDistanceLowerLimit = 1;
        private const float viewingDistanceUpperLimit = 100;


        // Fields.

        // GameObject components.
        private Camera cameraComponent;

        // Current location.
        private Vector3 currentViewedPosition;
        private float currentViewingDistance;
        private Quaternion currentRotation;

        // Default location.
        private Vector3 defaultViewedPosition;
        private float defaultViewingDistance;
        private Quaternion defaultRotation;

#pragma warning disable IDE0044, CS0414, CS0649

        [Header("Components")]
        [SerializeField]
        private GameObject cameraHolder;

        [Header("Parameters")]
        [SerializeField]
        private bool isControlledByInput = false;
        [SerializeField]
        private bool followSurface = false;

        [Space]
        [SerializeField]
        [Range(1f, 100.0f)]
        private float motionSpeed = 10.0f;
        [SerializeField]
        [Range(10f, 1000.0f)]
        private float zoomingSpeed = 100.0f;
        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float rotationSpeed = 0.1f;

        [Space]
        [SerializeField]
        [Range(viewingDistanceLowerLimit, viewingDistanceUpperLimit)]
        private float minViewingDistance = 10.0f;
        [SerializeField]
        [Range(viewingDistanceLowerLimit, viewingDistanceUpperLimit)]
        private float maxViewingDistance = 20.0f;

#pragma warning restore IDE0044, CS0414, CS0649

        private void OnValidate() {

            // Validate speed parameters.
            motionSpeed = Mathf.Max(motionSpeed, 0f);
            rotationSpeed = Mathf.Max(rotationSpeed, 0f);
            zoomingSpeed = Mathf.Max(zoomingSpeed, 0f);

            // Validate height parameters.
            minViewingDistance = Mathf.Clamp(minViewingDistance, viewingDistanceLowerLimit, viewingDistanceUpperLimit);
            maxViewingDistance = Mathf.Clamp(maxViewingDistance, minViewingDistance, viewingDistanceUpperLimit);

        }


        // Properties.

        public Camera CameraComponent => cameraComponent;

        public float CurrentViewingDistance {
            get => currentViewingDistance;
            set => currentViewingDistance = value;
        }


        // Life cycle.

        private void Awake() {
            defaultViewedPosition = Vector3.zero;
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

        private void LateUpdate() {

            // Update current location.
            float deltaTime = Time.unscaledDeltaTime;
            if (isControlledByInput && BattlefieldController.Instance.BattleStarted) {

                // Process input.
                if (Input.GetAxisRaw("Cancel") == 0) {

                    // Update viewed position.
                    Vector3 positionChange = Vector3.zero;
                    positionChange += transform.right * Input.GetAxisRaw("Common X");
                    positionChange += transform.forward * Input.GetAxisRaw("Common Y");
                    positionChange *= motionSpeed * deltaTime;
                    currentViewedPosition += positionChange;

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

            ApplyCurrentLocation();

        }


        // Methods.

        public void Center(Vector3 position) {
            currentViewedPosition = position;
            ApplyCurrentLocation();
        }

        public void Center(UnitController unit) {
            if (unit == null) {
                return;
            }
            currentViewedPosition = unit.Position;
            ApplyCurrentLocation();
        }


        // Support.

        private void ApplyCurrentLocation() {

            // Correct location.
            CorrectViewedPosition(ref currentViewedPosition);
            CorrectViewingDistance(ref currentViewingDistance);

            // Apply location.
            transform.rotation = currentRotation;
            Vector3 offset = currentViewingDistance * -cameraHolder.transform.forward;
            transform.position = currentViewedPosition + offset;

        }

        private void CorrectViewedPosition(ref Vector3 position) {
            Vector2Int mapSize = MapController.Instance.Size;
            position.x = Mathf.Clamp(position.x, -1, mapSize.x);
            position.z = Mathf.Clamp(position.z, -1, mapSize.y);
            if (followSurface) {
                position.y = Mathf.Max(MapController.Instance.GetSurfaceHeight(position), 0);
            }
        }

        private void CorrectViewingDistance(ref float distance) {
            distance = Mathf.Clamp(distance, minViewingDistance, maxViewingDistance);
        }


    }

}

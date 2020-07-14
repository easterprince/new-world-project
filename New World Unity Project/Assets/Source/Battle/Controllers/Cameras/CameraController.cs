using NewWorld.Battle.Controllers.Battlefield;
using NewWorld.Battle.Controllers.Map;
using NewWorld.Battle.Cores.Unit;
using NewWorld.Utilities;
using System;
using UnityEngine;

namespace NewWorld.Battle.Controllers.Cameras {

    public class CameraController : MonoBehaviour {

        // Constants.

        private const float viewingDistanceLowerLimit = 1;
        private const float viewingDistanceUpperLimit = 100;


        // Fields.

        // Components.
        private Camera cameraComponent;

        // Location.
        private CameraLocation currentLocation;
        private CameraLocation defaultLocation;

        // References.
        [Header("References")]
        [SerializeField]
        private GameObject cameraHolder;
        [Space]
        [SerializeField]
        private BattlefieldController battlefield;
        [SerializeField]
        private MapController map;

        // Parameters.
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

        private void OnValidate() {

            // Fix location.
            CurrentLocation = currentLocation;

            // Fix speed parameters.
            MotionSpeed = motionSpeed;
            ZoomingSpeed = zoomingSpeed;
            RotationSpeed = rotationSpeed;

            // Fix height parameters.
            MinViewingDistance = minViewingDistance;
            MaxViewingDistance = maxViewingDistance;

        }


        // Properties.

        public CameraLocation CurrentLocation {
            get => currentLocation;
            set {
                value.ViewingDistance = Mathf.Clamp(value.ViewingDistance, minViewingDistance, maxViewingDistance);
                currentLocation = value;
            }
        }

        public CameraLocation DefaultLocation {
            get => defaultLocation;
            set => defaultLocation = value;
        }

        public Camera CameraComponent => cameraComponent;

        public GameObject CameraHolder {
            get => cameraHolder;
            set => cameraHolder = value;
        }

        public BattlefieldController Battlefield {
            get => battlefield;
            set => battlefield = value;
        }

        public MapController Map {
            get => map;
            set => map = value;
        }

        public bool IsControlledByInput {
            get => isControlledByInput;
            set => isControlledByInput = value;
        }

        public bool FollowSurface {
            get => followSurface;
            set => followSurface = value;
        }

        public float MotionSpeed {
            get => motionSpeed;
            set => motionSpeed = Mathf.Max(value, 0f);
        }

        public float ZoomingSpeed {
            get => zoomingSpeed;
            set => zoomingSpeed = Mathf.Max(value, 0f);
        }

        public float RotationSpeed {
            get => rotationSpeed;
            set => rotationSpeed = Mathf.Max(value, 0f);
        }

        public float MinViewingDistance {
            get => minViewingDistance;
            set {
                minViewingDistance = Mathf.Clamp(value, viewingDistanceLowerLimit, maxViewingDistance);
                CurrentLocation = currentLocation;
            }
        }

        public float MaxViewingDistance {
            get => maxViewingDistance;
            set {
                maxViewingDistance = Mathf.Clamp(value, viewingDistanceLowerLimit, viewingDistanceUpperLimit);
                MinViewingDistance = minViewingDistance;
            }
        }


        // Life cycle.

        private void Awake() {
            defaultLocation.ViewingDistance = maxViewingDistance;
            defaultLocation.Rotation = transform.rotation;
            DefaultLocation = defaultLocation;
            CurrentLocation = defaultLocation;
        }

        private void Start() {
            GameObjects.ValidateReference(cameraHolder, nameof(cameraHolder));
            cameraComponent = cameraHolder.GetComponent<Camera>();
            GameObjects.ValidateComponent(cameraComponent, nameof(cameraHolder));
        }

        private void LateUpdate() {

            // Process input.
            float deltaTime = Time.unscaledDeltaTime;
            if (isControlledByInput && (battlefield == null || battlefield.Started)) {

                // Process input.
                if (Input.GetAxisRaw("Cancel") == 0) {

                    // Update viewed position.
                    Vector3 positionChange = Vector3.zero;
                    positionChange += transform.right * Input.GetAxisRaw("Common X");
                    positionChange += transform.forward * Input.GetAxisRaw("Common Y");
                    positionChange *= motionSpeed * deltaTime;
                    currentLocation.ViewedPosition += positionChange;

                    // Update distance.
                    float distanceChange = zoomingSpeed * deltaTime * -Input.GetAxisRaw("Common Z");
                    currentLocation.ViewingDistance += distanceChange;

                    // Update rotation.
                    Quaternion additionalRotation = Quaternion.AngleAxis(360 * rotationSpeed * deltaTime * Input.GetAxisRaw("Turn"), transform.up);
                    currentLocation.Rotation *= additionalRotation;

                } else {

                    // Reset location.
                    currentLocation = defaultLocation;

                }
                CurrentLocation = currentLocation;

            }

            // Set height.
            if (followSurface && map != null && map.Presentation != null) {
                var position = currentLocation.ViewedPosition;
                float height = Mathf.Max(map.Presentation.GetHeight(position), 0);
                position.y = height;
                currentLocation.ViewedPosition = position;
            }

            // Apply location.
            transform.rotation = currentLocation.Rotation;
            Vector3 offset = currentLocation.ViewingDistance * -cameraHolder.transform.forward;
            transform.position = currentLocation.ViewedPosition + offset;

        }


    }

}

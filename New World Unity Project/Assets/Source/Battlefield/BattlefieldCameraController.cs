using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield {

    public class BattlefieldCameraController : SceneSingleton<BattlefieldCameraController> {

        // Fields.

        private Camera cameraComponent;

        private float zOffset;
        private Vector2 defaultRealPosition;
        private Vector2 currentRealPosition;
        private int currentDirection;
        private float defaultSize;
        private float currentSize;

#pragma warning disable IDE0044, CS0414

        [SerializeField]
        private float motionSpeedModifier = 1.0f;

        [SerializeField]
        private float scrollingSpeedModifier = 1.0f;

        [SerializeField]
        private float minCameraSize = 3.0f;

        [SerializeField]
        private float maxCameraSize = 10.0f;

#pragma warning restore IDE0044, CS0414

        private void OnValidate() {
            const float sizeLowerLimit = 1;
            const float sizeUpperLimit = 1000;
            minCameraSize = Mathf.Max(minCameraSize, sizeLowerLimit);
            maxCameraSize = Mathf.Max(maxCameraSize, sizeLowerLimit);
            minCameraSize = Mathf.Min(minCameraSize, sizeUpperLimit);
            maxCameraSize = Mathf.Min(maxCameraSize, sizeUpperLimit);
            minCameraSize = Mathf.Min(minCameraSize, maxCameraSize);
        }


        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
            cameraComponent = GetComponent<Camera>();
            zOffset = transform.position.z;
            defaultRealPosition = Vector2.zero;
            currentRealPosition = defaultRealPosition;
            currentDirection = 0;
            defaultSize = cameraComponent.orthographicSize;
            currentSize = defaultSize;
        }

        private void Update() {

            // Input processing.
            if (Input.GetAxis("Cancel") == 0) {
                int xDirection = BattlefieldComposition.GetNextClockwiseDirection(currentDirection);
                int yDirection = currentDirection;
                Vector2 realPositionAddition = Vector2.zero;
                realPositionAddition += ((Vector2) BattlefieldComposition.GetDirectionDelta(xDirection)).normalized * Input.GetAxis("Common X");
                realPositionAddition += ((Vector2) BattlefieldComposition.GetDirectionDelta(yDirection)).normalized * Input.GetAxis("Common Y");
                realPositionAddition *= motionSpeedModifier;
                currentRealPosition += realPositionAddition;
                currentSize += scrollingSpeedModifier * -Input.GetAxis("Common Z");
                currentSize = Mathf.Clamp(currentSize, minCameraSize, maxCameraSize);
            } else {
                currentRealPosition = defaultRealPosition;
                currentSize = defaultSize;
            }

            // Properties updating.
            UpdateCameraLocation();
        }


        // Methods.

        public void Place(Vector2 newRealPosition) {
            currentRealPosition = newRealPosition;
            UpdateCameraLocation();
        }

        public void Rotate(int newDirection) {
            currentDirection = newDirection;
            UpdateCameraLocation();
        }


        // Support.

        private void UpdateCameraLocation() {
            Vector3 updatedPosition = BattlefieldComposition.RealToVisible(currentRealPosition, currentDirection);
            updatedPosition.z = zOffset;
            transform.position = updatedPosition;
            cameraComponent.orthographicSize = currentSize;
        }


    }

}

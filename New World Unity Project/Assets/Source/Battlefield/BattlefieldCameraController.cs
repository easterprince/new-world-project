using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NewWorld.Utilities;
using NewWorld.Utilities.Singletones;

namespace NewWorld.Battlefield {

    public class BattlefieldCameraController : SceneSingleton<BattlefieldCameraController> {

        // Fields.

        private Camera cameraComponent;

        private Vector3 defaultPosition;
        private Vector3 currentPosition;
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
            defaultPosition = transform.position;
            currentPosition = defaultPosition;
            defaultSize = cameraComponent.orthographicSize;
            currentSize = defaultSize;
        }

        private void Update() {
            if (Input.GetAxis("Cancel") == 0) {
                currentPosition.x += motionSpeedModifier * Input.GetAxis("Common X");
                currentPosition.y += motionSpeedModifier * Input.GetAxis("Common Y");
                currentSize += scrollingSpeedModifier * -Input.GetAxis("Common Z");
                currentSize = Mathf.Clamp(currentSize, minCameraSize, maxCameraSize);
            } else {
                currentPosition = defaultPosition;
                currentSize = defaultSize;
            }
            transform.position = currentPosition;
            cameraComponent.orthographicSize = currentSize;
        }

        // TODO: Implement rotation.

    }

}

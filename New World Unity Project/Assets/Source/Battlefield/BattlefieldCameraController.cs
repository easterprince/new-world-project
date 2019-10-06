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
        private Quaternion defaultRotation;
        private float defaultSize;

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private float motionSpeedModifier = 1.0f;

        [SerializeField]
        private float rotationSpeedModifier = 1.0f;

        [SerializeField]
        private float scrollingSpeedModifier = 1.0f;

        [SerializeField]
        private float minCameraSize = 3.0f;

        [SerializeField]
        private float maxCameraSize = 10.0f;

#pragma warning restore IDE0044, CS0414, CS0649

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
            defaultRotation = transform.rotation;
            defaultSize = cameraComponent.orthographicSize;
        }

        private void Update() {
            if (BattlefieldController.Instance.BattleStarted) {

                // Input processing.
                if (Input.GetAxisRaw("Cancel") == 0) {
                    Vector3 positionAddition = Vector3.zero;
                    positionAddition += transform.right * Input.GetAxisRaw("Common X");
                    positionAddition += transform.forward * Input.GetAxisRaw("Common Y");
                    positionAddition *= motionSpeedModifier;
                    transform.position += positionAddition;
                    transform.rotation *= Quaternion.Euler(0, Input.GetAxisRaw("Turn") * rotationSpeedModifier, 0);
                    float newSize = cameraComponent.orthographicSize + scrollingSpeedModifier * -Input.GetAxisRaw("Common Z");
                    cameraComponent.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);
                } else {
                    transform.position = defaultPosition;
                    transform.rotation = defaultRotation;
                    cameraComponent.orthographicSize = defaultSize;
                }

            }
        }


    }

}

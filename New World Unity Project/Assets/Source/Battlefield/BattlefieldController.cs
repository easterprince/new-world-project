using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Composition;

namespace NewWorld.Battlefield {

    public class BattlefieldController : SceneSingleton<BattlefieldController> {

        // Variables.

        private int currentDirection;
        private int lastTurnInput;


        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
            currentDirection = 0;
            lastTurnInput = 0;
        }

        private void Update() {
            int newTurnInput = System.Math.Sign(Input.GetAxis("Turn"));
            if (newTurnInput != lastTurnInput) {
                lastTurnInput = newTurnInput;
                if (newTurnInput > 0) {
                    currentDirection = VisionDirections.GetNextClockwiseDirection(currentDirection);
                } else if (newTurnInput < 0) {
                    currentDirection = VisionDirections.GetNextCounterclockwiseDirection(currentDirection);
                }
                if (newTurnInput != 0) {
                    Map.MapController.Instance?.Rotate(currentDirection);
                    BattlefieldCameraController.Instance.Rotate(currentDirection);
                }
            }
        }

    }

}

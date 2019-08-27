using System.Collections.Generic;
using UnityEngine;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Composition;

namespace NewWorld.Battlefield {

    public class BattlefieldController : SceneSingleton<BattlefieldController> {

        // Variables.

#pragma warning disable IDE0044, CS0414

        [SerializeField]
        private Map.MapController map = null;

#pragma warning restore IDE0044, CS0414

        private bool battleStarted;
        private int currentDirection;
        private int lastTurnInput;


        // Properties.

        public bool BattleStarted => battleStarted;


        // Life cycle.

        protected override void Awake() {
            base.Awake();
            Instance = this;
            battleStarted = false;
            currentDirection = 0;
            lastTurnInput = 0;
        }

        private void Update() {

            // Read rotation commands.
            if (battleStarted) {
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


        // Controlling methods.

        public void LoadBattle() {
            map.gameObject.SetActive(true);
        }

        public void StartBattle() {
            battleStarted = true;
            BattlefieldCameraController.Instance.Place(Vector3.zero);
        }


    }

}

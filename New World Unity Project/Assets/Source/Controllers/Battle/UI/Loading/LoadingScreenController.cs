using NewWorld.Controllers.Battle.Battlefield;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Controllers.Battle.UI.Loading {

    public class LoadingScreenController : SteadyController {

        // Fields.

        // Battlefield controller reference.
        [SerializeField]
        private BattlefieldController battlefield;

        // Child references. 
        [SerializeField]
        private GameObject readyTextPanel;
        [SerializeField]
        private LoadingLogoController logo;
        [SerializeField]
        private Text loadingProgress;


        // Properties.

        public BattlefieldController Battlefield {
            get => battlefield;
            set {
                ValidateBeingNotStarted();
                battlefield = value;
                UpdateThings();
            }
        }

        public GameObject ReadyTextPanel {
            get => readyTextPanel;
            set {
                ValidateBeingNotStarted();
                readyTextPanel = value;
                UpdateThings();
            }
        }

        public LoadingLogoController Logo {
            get => logo;
            set {
                ValidateBeingNotStarted();
                logo = value;
                UpdateThings();
            }
        }


        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();

            // Validate references.
            GameObjects.ValidateReference(battlefield, nameof(battlefield));
            GameObjects.ValidateReference(readyTextPanel, nameof(readyTextPanel));
            GameObjects.ValidateReference(logo, nameof(logo));

            // Set up event handlers.
            battlefield.BattleStatusChangedEvent.AddAction(this, (status) => UpdateThings());
            battlefield.LoadingStatusChangedEvent.AddAction(this, (status) => UpdateThings());

        }

        private void LateUpdate() {
            if (battlefield.Status == BattlefieldController.BattleStatus.Ready && Input.anyKey) {
                battlefield.Paused = false;
                gameObject.SetActive(false);
            }
        }


        // Support method.

        private void UpdateThings() {
            if (battlefield.Status == BattlefieldController.BattleStatus.Inactive) {
                readyTextPanel.SetActive(false);
                logo.CurrentCondition = LoadingLogoController.Condition.Waiting;
                loadingProgress.text = "...";
            } else if (battlefield.Status == BattlefieldController.BattleStatus.Loading) {
                readyTextPanel.SetActive(false);
                logo.CurrentCondition = LoadingLogoController.Condition.Loading;
                loadingProgress.text = battlefield.LoadingStatus;
            } else {
                readyTextPanel.SetActive(true);
                logo.CurrentCondition = LoadingLogoController.Condition.Ready;
                loadingProgress.text = battlefield.LoadingStatus;
            }
        }


    }

}

using NewWorld.Controllers.Battle.Battlefield;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Controllers.Battle.UI.Loading {

    public class LoadingScreenController : MonoBehaviour {

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
                battlefield = value;
                UpdateThings();
            }
        }

        public GameObject ReadyTextPanel {
            get => readyTextPanel;
            set {
                readyTextPanel = value;
                UpdateThings();
            }
        }

        public LoadingLogoController Logo {
            get => logo;
            set {
                logo = value;
                UpdateThings();
            }
        }

        public Text LoadingProgress {
            get => loadingProgress;
            set => loadingProgress = value;
        }


        // Life cycle.

        private void LateUpdate() {
            UpdateThings();
            if (battlefield != null && battlefield.FinishedBuilding && Input.anyKey) {
                battlefield.Paused = false;
                gameObject.SetActive(false);
            }
        }


        // Support method.

        private void UpdateThings() {
            if (battlefield == null || !battlefield.StartedBuilding) {
                if (readyTextPanel != null) {
                    readyTextPanel.SetActive(false);
                }
                if (logo != null) {
                    logo.CurrentCondition = LoadingLogoController.Condition.Waiting;
                }
                if (loadingProgress != null) {
                    loadingProgress.text = "...";
                }
            } else if (battlefield.StartedBuilding && !battlefield.FinishedBuilding) {
                if (readyTextPanel != null) {
                    readyTextPanel.SetActive(false);
                }
                if (logo != null) {
                    logo.CurrentCondition = LoadingLogoController.Condition.Loading;
                }
                if (loadingProgress != null) {
                    loadingProgress.text = battlefield.LoadingStatus;
                }
            } else {
                if (readyTextPanel != null) {
                    readyTextPanel.SetActive(true);
                }
                if (logo != null) {
                    logo.CurrentCondition = LoadingLogoController.Condition.Ready;
                }
                if (loadingProgress != null) {
                    loadingProgress.text = battlefield.LoadingStatus;
                }
            }
        }


    }

}

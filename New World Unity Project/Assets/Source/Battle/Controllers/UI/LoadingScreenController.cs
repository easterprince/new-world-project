using NewWorld.Battle.Controllers.Battlefield;
using UnityEngine;

namespace NewWorld.Battle.Controllers.UI {
    
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


        // Properties.

        public BattlefieldController Battlefield {
            get => battlefield;
            set {
                battlefield = value;
                LateUpdate();
            }
        }

        public GameObject ReadyTextPanel {
            get => readyTextPanel;
            set {
                readyTextPanel = value;
                LateUpdate();
            }
        }

        public LoadingLogoController Logo {
            get => logo;
            set {
                logo = value;
                LateUpdate();
            }
        }


        // Life cycle.

        private void LateUpdate() {
            UpdateThings();
            if (battlefield != null && battlefield.Ready && Input.anyKey) {
                gameObject.SetActive(false);
            }
        }


        // Support method.

        private void UpdateThings() {
            if (battlefield == null) {
                if (readyTextPanel != null) {
                    readyTextPanel.SetActive(false);
                }
                if (logo != null) {
                    logo.CurrentCondition = LoadingLogoController.Condition.Waiting;
                }
            } else if (!battlefield.Ready) {
                if (readyTextPanel != null) {
                    readyTextPanel.SetActive(false);
                }
                if (logo != null) {
                    logo.CurrentCondition = LoadingLogoController.Condition.Loading;
                }
            } else {
                if (readyTextPanel != null) {
                    readyTextPanel.SetActive(true);
                }
                if (logo != null) {
                    logo.CurrentCondition = LoadingLogoController.Condition.Ready;
                }
            }
        }


    }

}

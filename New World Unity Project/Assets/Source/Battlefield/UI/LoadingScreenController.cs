using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewWorld.Battlefield.UI {

    public class LoadingScreenController : MonoBehaviour {

        // Fields.

#pragma warning disable IDE0044, CS0414, CS0649

        [SerializeField]
        private GameObject readyTextPanel;

        [SerializeField]
        private LoadingLogoController logo;

#pragma warning restore IDE0044, CS0414, CS0649

        bool ready = false;
        

        // Life cycle.

        private void Awake() {
            if (readyTextPanel == null) {
                throw new MissingReferenceException($"Missing {nameof(readyTextPanel)}.");
            }
            if (logo == null) {
                throw new MissingReferenceException($"Missing {nameof(logo)}.");
            }
            readyTextPanel.SetActive(false);
        }

        private void Start() {
            logo.CurrentCondition = LoadingLogoController.Condition.Waiting;
            
            BattlefieldController.EnsureInstance(this);
            BattlefieldController.Instance.UnloadedEvent.AddListener(WhenUnloaded);

            void WhenUnloaded() {
                BattlefieldController.Instance.UnloadedEvent.RemoveListener(WhenUnloaded);
                BattlefieldController.Instance.LoadedEvent.AddListener(WhenLoaded);
                logo.CurrentCondition = LoadingLogoController.Condition.Loading;
            }

            void WhenLoaded() {
                BattlefieldController.Instance.LoadedEvent.RemoveListener(WhenLoaded);
                logo.CurrentCondition = LoadingLogoController.Condition.Ready;
                readyTextPanel.SetActive(true);
                ready = true;
            }

        }

        private void Update() {
            if (ready && Input.anyKey) {
                gameObject.SetActive(false);
                BattlefieldController.Instance.StartBattle();
            }
        }


    }

}

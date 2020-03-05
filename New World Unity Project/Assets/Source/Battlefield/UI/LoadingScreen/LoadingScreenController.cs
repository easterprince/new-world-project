using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewWorld.Utilities;

namespace NewWorld.Battlefield.UI.LoadingScreen {

    public class LoadingScreenController : MonoBehaviour {

        // Fields.

        // Child references. 
#pragma warning disable IDE0044, CS0414, CS0649
        [SerializeField]
        private GameObject readyTextPanel;
        [SerializeField]
        private LoadingLogoController logo;
#pragma warning restore IDE0044, CS0414, CS0649

        // Structure.
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
            BattlefieldController.Instance.UnloadedEvent.AddListener(WhenBattlefieldUnloaded);

        }

        private void Update() {
            if (ready && Input.anyKey) {
                gameObject.SetActive(false);
                BattlefieldController.Instance.StartBattle();
            }
        }

        private void OnDestroy() {
            BattlefieldController.Instance?.UnloadedEvent.RemoveListener(WhenBattlefieldUnloaded);
            BattlefieldController.Instance?.LoadedEvent.RemoveListener(WhenBattlefieldLoaded);
        }


        // Event handlers.

        private void WhenBattlefieldUnloaded() {
            BattlefieldController.Instance.UnloadedEvent.RemoveListener(WhenBattlefieldUnloaded);
            BattlefieldController.Instance.LoadedEvent.AddListener(WhenBattlefieldLoaded);
            logo.CurrentCondition = LoadingLogoController.Condition.Loading;
        }

        private void WhenBattlefieldLoaded() {
            BattlefieldController.Instance.LoadedEvent.RemoveListener(WhenBattlefieldLoaded);
            logo.CurrentCondition = LoadingLogoController.Condition.Ready;
            readyTextPanel.SetActive(true);
            ready = true;
        }


    }

}

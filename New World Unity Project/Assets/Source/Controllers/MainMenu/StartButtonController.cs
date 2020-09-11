using NewWorld.Cores.Battle.Battlefield;
using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NewWorld.Controllers.MainMenu {
    
    public class StartButtonController : SteadyController {

        // Fields.

        private Button button;


        // Life cycle.

        private protected override void OnStart() {
            base.OnStart();
            button = GetComponent<Button>();
            GameObjects.ValidateComponent(button);
            button.onClick.AddListener(StartBattleScene);
        }

        private void OnDestroy() {
            button.onClick.RemoveListener(StartBattleScene);
        }


        // Battle loading.

        private void StartBattleScene() {
            if (GameManager.DoesTransit) {
                return;
            }
            GameManager.StartBattleTransition((progress, cancellation) => Task.Run(() => null as BattlefieldCore));
        }


    }

}

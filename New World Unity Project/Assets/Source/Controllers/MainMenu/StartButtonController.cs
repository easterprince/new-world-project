using NewWorld.Utilities;
using NewWorld.Utilities.Controllers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NewWorld.Controllers.MainMenu {
    
    public class StartButtonController : SteadyController {

        // Constants.

        private const string battleSceneName = "Battle";


        // Fields.

        private bool used = false;
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

        private void StartBattleScene() {
            if (used) {
                return;
            }
            used = true;
            SceneManager.LoadScene(battleSceneName, LoadSceneMode.Additive);
        }


    }

}

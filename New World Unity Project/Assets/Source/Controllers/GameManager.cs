using NewWorld.Cores.Battle.Battlefield;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace NewWorld.Controllers {

    public static class GameManager {

        // Delegates.

        public delegate Task<BattlefieldCore> BattlefieldGenerationAsync(
            IProgress<string> progress, CancellationToken cancellationToken);


        // Constants.

        private const string battleSceneName = "Battle";


        // Fields.

        private static bool doesTransit = false;
        private static BattlefieldGenerationAsync battlefieldGenerationAsync = null;


        // Properties.

        public static bool DoesTransit => doesTransit;


        // Methods.

        public static void StartBattleTransition(BattlefieldGenerationAsync battlefieldGenerationAsync) {
            if (battlefieldGenerationAsync is null) {
                throw new ArgumentNullException(nameof(battlefieldGenerationAsync));
            }
            if (doesTransit) {
                throw new InvalidOperationException("Transition is already happenning.");
            }
            doesTransit = true;
            GameManager.battlefieldGenerationAsync = battlefieldGenerationAsync;
            SceneManager.LoadScene(battleSceneName);
        }

        public static BattlefieldGenerationAsync FinishBattleTransition() {
            if (GameManager.battlefieldGenerationAsync is null) {
                throw new InvalidOperationException("There is no battle transition happening.");
            }
            doesTransit = false;
            var battlefieldGenerationAsync = GameManager.battlefieldGenerationAsync;
            GameManager.battlefieldGenerationAsync = null;
            return battlefieldGenerationAsync;
        }


    }

}

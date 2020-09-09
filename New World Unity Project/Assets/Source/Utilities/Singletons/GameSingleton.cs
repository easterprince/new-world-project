namespace NewWorld.Utilities.Singletons {

    public class GameSingleton<TSelf> : SceneSingleton<TSelf>
        where TSelf : GameSingleton<TSelf> {

        override private protected void Awake() {
            base.Awake();
            DontDestroyOnLoad(this);
        }

    }

}

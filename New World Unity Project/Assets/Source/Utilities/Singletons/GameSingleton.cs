using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities.Singletons {

    public class GameSingleton<TSelf> : SceneSingleton<TSelf>
        where TSelf : GameSingleton<TSelf> {

        override private protected void Awake() {
            base.Awake();
            DontDestroyOnLoad(this);
        }

    }

}

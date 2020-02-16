using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities.Singletons {

    public class GameSingleton<T> : SceneSingleton<T>
        where T : GameSingleton<T> {

        override private protected void Awake() {
            base.Awake();
            DontDestroyOnLoad(this);
        }

    }

}

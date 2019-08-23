using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Utilities.Singletones {

    public class GameSingleton<T> : SceneSingleton<T>
        where T : GameSingleton<T> {

        override protected void Awake() {
            base.Awake();
            DontDestroyOnLoad(this);
        }

    }

}

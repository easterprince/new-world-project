using System;

namespace NewWorld.Battle.Cores {

    public abstract class GameActionBase<TSelf> : IGameAction
        where TSelf : GameActionBase<TSelf> {

        // Properties.

        public Type ActionType => typeof(TSelf);


    }

}

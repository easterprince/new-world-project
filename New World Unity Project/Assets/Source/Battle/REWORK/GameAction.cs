using System;

namespace NewWorld.Battlefield {

    public abstract class GameAction {
    
        // Methods.

        public abstract Type ActionType { get; }


    }

    public abstract class GameAction<TSelf> : GameAction
        where TSelf : GameAction<TSelf> {

        // Properties.

        override public Type ActionType => typeof(TSelf);


    }

}

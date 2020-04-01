using System;

namespace NewWorld.Battlefield {

    public abstract class GameAction {
    
        // Methods.

        public abstract Type ActionType { get; }


    }

    public abstract class GameAction<TSelf> : GameAction {

        // Static.

        public static Type UsedActionType => typeof(TSelf);


        // Methods.

        override public Type ActionType => UsedActionType;


    }

}

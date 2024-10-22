﻿using NewWorld.Utilities;

namespace NewWorld.Cores.Battle.Unit.Behaviours {

    public class IdleGoal : UnitGoal {

        // Static.

        private static readonly IdleGoal instance = new IdleGoal();

        public static IdleGoal Instance => instance;


        // Constructors.

        private IdleGoal() {}


        // Properties.

        public override NamedId Id => NamedId.Get("IdleGoal");


    }

}

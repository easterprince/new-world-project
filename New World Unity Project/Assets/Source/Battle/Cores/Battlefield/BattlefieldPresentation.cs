using NewWorld.Battle.Cores.Layout;
using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;
using System;

namespace NewWorld.Battle.Cores.Battlefield {

    public class BattlefieldPresentation : PresentationBase<BattlefieldCore>, IContextPointer {

        // Constructor.

        public BattlefieldPresentation(BattlefieldCore presented) : base(presented) {}


        // Properties.

        public float GameTime => Presented.GameTime;
        public float GameTimeDelta => Presented.GameTimeDelta;
        public MapPresentation Map => Presented.Map;
        public LayoutPresentation Layout => Presented.Layout;
        public UnitSystemPresentation UnitSystem => Presented.UnitSystem;


        // Methods.

        public void ExecuteAfterUpdate(Action action) => Presented.ExecuteAfterUpdate(action);


    }

}

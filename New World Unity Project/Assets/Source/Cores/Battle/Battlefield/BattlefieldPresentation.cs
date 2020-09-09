using NewWorld.Cores.Battle.Layout;
using NewWorld.Cores.Battle.Map;
using NewWorld.Cores.Battle.UnitSystem;
using System;

namespace NewWorld.Cores.Battle.Battlefield {

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

        public void ExecuteAfterUpdate(Action action) => Presented.ExecuteLater(action);


    }

}

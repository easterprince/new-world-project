using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;
using System;

namespace NewWorld.Battle.Cores.Battlefield {

    public class BattlefieldCore : CoreBase<BattlefieldPresentation> {

        // Fields.

        // Structure.
        private float gameTime = 0;
        private float gameTimeDelta = 0;

        // Subcores.
        private readonly MapCore map;
        private readonly UnitSystemCore unitSystem;


        // Constructor.

        public BattlefieldCore() {
            map = new MapCore(Presentation);
            unitSystem = new UnitSystemCore(Presentation);
        }


        // Properties.

        public float GameTime {
            get => gameTime;
            set => gameTime = value;
        }

        public float GameTimeDelta {
            get => gameTimeDelta;
            set => gameTimeDelta = value;
        }

        public MapPresentation Map => map?.Presentation;
        public UnitSystemPresentation UnitSystem => unitSystem?.Presentation;
        public override BattlefieldPresentation Context => Presentation;


        // Presentation generation.

        private protected override BattlefieldPresentation BuildPresentation() {
            return new BattlefieldPresentation(this);
        }


        // Updates.

        public void Update() {
            gameTime += gameTimeDelta;
            unitSystem.Update();
        }


        // Action planning.

        public void ExecuteAfterUpdate(Action action) {
            throw new NotImplementedException();
        }


    }

}

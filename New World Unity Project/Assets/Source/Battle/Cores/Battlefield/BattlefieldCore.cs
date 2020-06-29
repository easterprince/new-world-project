using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;
using System;

namespace NewWorld.Battle.Cores.Battlefield {

    public class BattlefieldCore : CoreBase<BattlefieldCore, BattlefieldPresentation> {

        // Fields.

        // Structure.
        private float gameTime = 0;
        private float gameTimeDelta = 0;

        // Subcores.
        private readonly MapCore map;
        private readonly UnitSystemCore unitSystem;


        // Constructors.

        public BattlefieldCore() {
            map = new MapCore();
            map.Connect(Presentation);
            unitSystem = new UnitSystemCore();
            unitSystem.Connect(Presentation);
        }

        public BattlefieldCore(BattlefieldCore other) {
            map = other.map.Clone();
            map.Connect(Presentation);
            unitSystem = other.unitSystem.Clone();
            unitSystem.Connect(Presentation);
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


        // Cloning.

        public override BattlefieldCore Clone() {
            return new BattlefieldCore(this);
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

using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;
using NewWorld.Utilities.Events;
using System;
using System.Collections.Generic;

namespace NewWorld.Battle.Cores.Battlefield {

    public class BattlefieldCore : CoreBase<BattlefieldCore, BattlefieldPresentation> {

        // Fields.

        // Structure.
        private float gameTime = 0;
        private float gameTimeDelta = 0;
        private readonly ActionQueue actionQueue = new ActionQueue();

        // Subcores.
        private readonly MapCore map;
        private readonly UnitSystemCore unitSystem;


        // Constructors.

        public BattlefieldCore(MapCore map, UnitSystemCore unitSystem) {
            this.map = map?.Clone() ?? throw new ArgumentNullException(nameof(map));
            this.map.Connect(Presentation);
            this.unitSystem = unitSystem?.Clone() ?? throw new ArgumentNullException(nameof(unitSystem));
            this.unitSystem.Connect(Presentation);
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

        public MapPresentation Map => map.Presentation;
        public UnitSystemPresentation UnitSystem => unitSystem.Presentation;
        public sealed override BattlefieldPresentation Context => Presentation;


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

            // Game time update.
            gameTime += gameTimeDelta;

            // Update of game entities.
            actionQueue.Clear();
            unitSystem.Update();

            // Execution of planned game actions.
            actionQueue.RunAll();

        }


        // Action planning.

        public void ExecuteAfterUpdate(Action action) {
            actionQueue.Enqueue(action);
        }


    }

}

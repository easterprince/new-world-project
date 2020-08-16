using NewWorld.Battle.Cores.Layout;
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
        private readonly LayoutCore layout;
        private readonly UnitSystemCore unitSystem;


        // Constructors.

        public BattlefieldCore(MapCore map, LayoutCore layout, UnitSystemCore unitSystem) {
            this.map = map?.Clone() ?? throw new ArgumentNullException(nameof(map));
            this.map.Connect(Presentation);
            this.layout = layout?.Clone() ?? throw new ArgumentNullException(nameof(layout));
            this.layout.Connect(Presentation);
            this.unitSystem = unitSystem?.Clone() ?? throw new ArgumentNullException(nameof(unitSystem));
            this.unitSystem.Connect(Presentation);
        }

        public BattlefieldCore(BattlefieldCore other) {
            map = other.map.Clone();
            map.Connect(Presentation);
            layout = other.layout.Clone();
            layout.Connect(Presentation);
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
        }

        public MapPresentation Map => map.Presentation;
        public LayoutPresentation Layout => layout.Presentation;
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

        public void Update(float gameTimeDelta) {

            // Update game time.
            this.gameTimeDelta = gameTimeDelta;
            gameTime += gameTimeDelta;

            // Execute externally obtained actions.
            actionQueue.RunAll();

            // Update game entities.
            unitSystem.Update();

            // Execute internally obtained actions.
            actionQueue.RunAll();

        }


        // Action planning.

        public void ExecuteLater(Action action) {
            actionQueue.Enqueue(action);
        }


    }

}

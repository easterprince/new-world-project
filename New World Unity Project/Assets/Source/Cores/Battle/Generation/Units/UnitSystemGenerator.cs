using NewWorld.Cores.Battle.Map;
using NewWorld.Cores.Battle.Unit;
using NewWorld.Cores.Battle.UnitSystem;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Cores.Battle.Generation.Units {

    public abstract class UnitSystemGenerator {

        // Fields.

        private MapPresentation map;
        private int unitCount;
        private UnitCore[] unitTemplates;


        // Properties.

        public MapPresentation Map {
            get => map;
            set => map = value;
        }

        public int UnitCount {
            get => unitCount;
            set => unitCount = Mathf.Max(value, 0);
        }

        public UnitCore[] UnitTemplates {
            get => unitTemplates;
            set => unitTemplates = value;
        }


        // Methods.

        public abstract UnitSystemCore Generate(int seed, CancellationToken? cancellationToken = null);

        public async Task<UnitSystemCore> GenerateAsync(int seed, CancellationToken? cancellationToken = null) {
            cancellationToken?.ThrowIfCancellationRequested();
            var task = Task.Run(() => Generate(seed, cancellationToken));
            var unitSystem = await task;
            return unitSystem;
        }


    }

}

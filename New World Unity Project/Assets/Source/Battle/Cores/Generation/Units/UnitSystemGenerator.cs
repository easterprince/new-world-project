﻿using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NewWorld.Battle.Cores.Generation.Units {

    public abstract class UnitSystemGenerator {

        // Fields.

        private MapPresentation map;
        private int unitCount;


        // Properties.

        public MapPresentation Map {
            get => map;
            set => map = value;
        }

        public int UnitCount {
            get => unitCount;
            set => unitCount = Mathf.Max(value, 0);
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

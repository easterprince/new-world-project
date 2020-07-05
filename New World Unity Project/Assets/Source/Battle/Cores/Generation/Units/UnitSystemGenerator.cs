using NewWorld.Battle.Cores.Map;
using NewWorld.Battle.Cores.UnitSystem;
using UnityEngine;

namespace NewWorld.Battle.Cores.Generation.Units {
    
    public abstract class UnitSystemGenerator {
    
        // Fields.

        private int unitCount;


        // Properties.

        public int UnitCount {
            get => unitCount;
            set => unitCount = Mathf.Max(value, 0);
        }


        // Methods.

        public abstract UnitSystemCore Generate(int seed, MapCore map);


    }

}

using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public interface IUnitModule {

        // Properties.

        bool Connected { get; }
        UnitController Owner { get; }


    }

}

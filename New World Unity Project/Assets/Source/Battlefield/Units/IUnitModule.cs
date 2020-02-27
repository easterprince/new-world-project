using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public interface IUnitModule {

        // Properties.

        UnitController Owner { get; }
        bool Connected { get; }


    }

}

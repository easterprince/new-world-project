using UnityEngine;

namespace NewWorld.Battlefield.Units {

    public interface IUnitModulePresentation {

        // Properties.

        UnitController Owner { get; }


        // Methods.

        bool BelongsTo(IUnitModule unitModule);


    }

}

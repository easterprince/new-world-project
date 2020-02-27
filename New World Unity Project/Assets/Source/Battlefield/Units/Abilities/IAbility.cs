using NewWorld.Battlefield.Units.Conditions;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities {

    public interface IAbility : IUnitModule {

        // Methods.
        ICondition Use(object parameterSet);
        void Connect(UnitController owner);
        new IAbilityPresentation Presentation { get; }


    }

}

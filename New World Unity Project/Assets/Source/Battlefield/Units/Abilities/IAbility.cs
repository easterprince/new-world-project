using NewWorld.Battlefield.Units.Conditions;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities {

    public interface IAbility : IUnitModule {

        // Properties.
        new IAbilityPresentation Presentation { get; }
        string Name { get; }


        // Methods.
        ICondition Use(object parameterSet);
        void Connect(UnitController owner);


    }

}

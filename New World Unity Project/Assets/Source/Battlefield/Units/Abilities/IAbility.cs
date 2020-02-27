using NewWorld.Battlefield.Units.Conditions;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities {

    public interface IAbility : IUnitModule {

        Condition Use(object parameterSet);

    }

}

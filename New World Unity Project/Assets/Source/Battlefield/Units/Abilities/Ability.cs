using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability : UnitModule {

        // Constructor.

        public Ability(UnitController owner) : base(owner) {}


        // Methods.

        public abstract ReadyCondition Use(object parameterSet);


    }

}
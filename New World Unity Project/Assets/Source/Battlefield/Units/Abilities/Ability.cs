using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability<PresentationType> : UnitModule<PresentationType>, IAbility {

        // Constructor.

        protected Ability() : base() {}


        // Methods.

        new public void Connect(UnitController owner) {
            base.Connect(owner);
        }

        public abstract Condition Use(object parameterSet);


    }

}
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability : UnitModule<UnitController> {

        // Properties.

        public virtual string Name => "Unknown Ability";


        // Constructor.

        protected Ability() : base() {}


        // Methods.

        public UnitCondition Use(ParentPassport<UnitController> parentPassport, object parameterSet) {
            ValidatePassport(parentPassport);
            if (!Connected) {
                throw new System.InvalidOperationException("Ability cannot be used when disconnected.");
            }
            return MakeCondition(parameterSet);
        }


        // Inner methods.

        protected private abstract UnitCondition MakeCondition(object parameterSet);


    }

}
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Unit.Actions;
using NewWorld.Battlefield.Unit.Conditions;

namespace NewWorld.Battlefield.Unit.Abilities {

    public abstract class UnitAbility : UnitModule<UnitController> {

        // Properties.

        public virtual string Name => "Unknown Ability";


        // Constructor.

        protected UnitAbility() : base() {}


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
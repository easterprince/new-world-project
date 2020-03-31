using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Unit.Actions;
using NewWorld.Battlefield.Unit.Conditions;
using NewWorld.Battlefield.Unit.Core;

namespace NewWorld.Battlefield.Unit.Abilities {

    public abstract class UnitAbility : UnitModule<UnitAbility, UnitCore, UnitAbilityPresentation> {

        // Properties.

        public virtual string Name => "Unknown Ability";


        // Constructor.

        protected UnitAbility() : base() {}


        // Methods.

        public UnitCondition Use(object parameterSet) {
            if (!Connected) {
                throw new System.InvalidOperationException("Ability cannot be used when disconnected.");
            }
            return MakeCondition(parameterSet);
        }


        // Inner methods.

        protected private abstract UnitCondition MakeCondition(object parameterSet);


    }

}
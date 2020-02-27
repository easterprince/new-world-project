using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability<TPresentation> : UnitModule<TPresentation>, IAbility
        where TPresentation : class, IAbilityPresentation {

        // Properties.

        IAbilityPresentation IAbility.Presentation => Presentation;


        // Constructor.

        protected Ability() : base() {}


        // Methods.

        new public void Connect(UnitController owner) {
            base.Connect(owner);
        }

        public Condition Use(object parameterSet) {
            if (!Connected) {
                throw new System.InvalidOperationException("Ability cannot be used when disconnected.");
            }
            return MakeCondition(parameterSet);
        }


        // Inner methods.

        protected abstract Condition MakeCondition(object parameterSet);


    }

}
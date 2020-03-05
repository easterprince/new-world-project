using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability<TPresentation> : UnitModule<TPresentation>, IAbility
        where TPresentation : class, IAbilityPresentation {

        // Properties.

        IAbilityPresentation IAbility.Presentation => Presentation;
        public virtual string Name => "Unknown";


        // Constructor.

        protected Ability() : base() {}


        // Methods.

        new public void Connect(UnitController owner) {
            base.Connect(owner);
        }

        public ICondition Use(object parameterSet) {
            if (!Connected) {
                throw new System.InvalidOperationException("Ability cannot be used when disconnected.");
            }
            return MakeCondition(parameterSet);
        }


        // Inner methods.

        protected abstract ICondition MakeCondition(object parameterSet);


    }

}
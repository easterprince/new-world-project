using UnityEngine;
using NewWorld.Battlefield.Units.Abilities;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.General {

    public class UseAbility : GeneralUnitUpdate {

        // Fields.

        private readonly IAbility ability;
        private readonly object parameterSet;


        // Properties.

        public IAbility Ability => ability;
        public object ParameterSet => parameterSet;


        // Constructor.

        public UseAbility(IAbility ability, object parameterSet) : base(ability.Owner) {
            this.ability = ability ?? throw new System.ArgumentNullException(nameof(ability));
            this.parameterSet = parameterSet;
        }


    }

}

using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Active {

    public abstract class ActiveAbility : Ability {

        // Properties.

        public abstract bool IsUsed { get; }


        // Constructor.

        public ActiveAbility(UnitController owner) : base(owner) { }


    }

}

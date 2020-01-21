using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Active {

    public abstract class UsableAbility : ActiveAbility {

        // Constructor.

        public UsableAbility(UnitController owner) : base(owner) { }


        // Methods.

        public abstract void Use();


    }

    public abstract class UsableAbility<UsageParameterType> : ActiveAbility {

        // Constructor.

        public UsableAbility(UnitController owner) : base(owner) { }


        // Methods.

        public abstract void Use(UsageParameterType parameter);


    }

}

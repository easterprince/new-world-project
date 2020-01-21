using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Active {

    public abstract class UsableActiveAbility : ActiveAbility {

        // Constructor.

        public UsableActiveAbility(UnitController owner) : base(owner) { }


        // Methods.

        public abstract void Use();


    }

    public abstract class UsableActiveAbility<UsageParameterType> : ActiveAbility {

        // Constructor.

        public UsableActiveAbility(UnitController owner) : base(owner) { }


        // Methods.

        public abstract void Use(UsageParameterType parameter);


    }

}

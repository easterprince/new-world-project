using System.Collections.Generic;
using NewWorld.Battlefield.Units.Core;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability : IIntending {

        // Fields.

        private readonly UnitAccount unitAccount;


        // Properties.

        protected UnitAccount UnitAccount => unitAccount;


        // Constructor.

        public Ability(UnitAccount unitAccount) {
            this.unitAccount = unitAccount ?? throw new System.ArgumentNullException(nameof(unitAccount));
        }


        // Methods.

        public abstract IEnumerable<Intention> ReceiveIntentions();

        public abstract void Fulfil(Intention intention);


    }

}
using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Core {

    public abstract class UnitCoreModule : IIntending {

        // Fields.

        private readonly UnitAccount unitAccount;


        // Properties.

        protected UnitAccount UnitAccount => unitAccount;


        // Constructor.

        public UnitCoreModule(UnitAccount unitAccount) {
            this.unitAccount = unitAccount ?? throw new System.ArgumentNullException(nameof(unitAccount));
        }


        // Methods.

        public abstract IEnumerable<Intention> ReceiveIntentions();

        public abstract void Fulfil(Intention intention);

    }

}

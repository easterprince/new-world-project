using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability : IActing {

        // Fields.

        private readonly UnitController owner;


        // Properties.

        protected UnitController Owner => owner;


        // Constructor.

        public Ability(UnitController owner) {
            this.owner = owner;
        }


        // Methods.

        public abstract IEnumerable<UnitAction> ReceiveActions();


    }

}
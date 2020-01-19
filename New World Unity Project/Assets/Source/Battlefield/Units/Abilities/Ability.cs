using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability : IActing {

        // Fields.

        private readonly UnitController owner;


        // Properties.

        protected UnitController Owner => owner;
        public abstract bool IsUsed { get; }


        // Constructor.

        public Ability(UnitController owner) {
            this.owner = owner;
        }


        // Methods.

        public abstract IEnumerable<GameAction> ReceiveActions();


    }

}
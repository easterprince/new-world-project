using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Actions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability : UnitModule, IActing {

        // Constructor.

        public Ability(UnitController owner) : base(owner) {}


        // Methods.

        public abstract IEnumerable<GameAction> ReceiveActions();


    }

}
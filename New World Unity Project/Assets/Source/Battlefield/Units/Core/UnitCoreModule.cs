using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Core {

    public abstract class UnitCoreModule : IIntending {

        // Methods.

        public abstract IEnumerable<Intention> ReceiveIntentions();

        public abstract void Fulfil(Intention intention);

    }

}

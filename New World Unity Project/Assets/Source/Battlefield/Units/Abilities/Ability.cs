using System.Collections.Generic;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Abilities {

    public abstract class Ability : IIntending {

        // Methods.

        public abstract IEnumerable<Intention> ReceiveIntentions();

        public abstract void Fulfil(Intention intention);


    }

}
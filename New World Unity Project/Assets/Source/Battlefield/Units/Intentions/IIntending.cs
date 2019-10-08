using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Intentions {

    public interface IIntending {

        // Methods.

        IEnumerable<Intention> ReceiveIntentions();

        void Fulfil(Intention intention);

    }

}

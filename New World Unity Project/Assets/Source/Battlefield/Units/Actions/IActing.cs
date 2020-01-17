using System.Collections.Generic;
using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions {

    public interface IActing {

        // Methods.

        IEnumerable<UnitAction> ReceiveActions();

    }

}

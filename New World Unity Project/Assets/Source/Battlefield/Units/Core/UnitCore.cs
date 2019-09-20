using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Core {

    public class UnitCore : IIntending {

        // Fields.

        private readonly UnitLocator locator;


        // Properties.

        public Vector2Int ConnectedNode => locator.ConnectedNode;


        // Interactions.

        public Vector3 GetPosition() {
            return locator.GetPosition();
        }


        // Constructor.

        public UnitCore(UnitDescription description) {
            if (description == null) {
                throw new System.ArgumentNullException(nameof(description));
            }
            locator = new UnitLocator(null, description.CurrentNode);
        }


        // Intentions management.

        public void Fulfil(Intention intention) {
            if (intention == null) {
                throw new System.ArgumentNullException(nameof(intention));
            }
            if (intention is UpdateConnectedNodeIntention updateConnectedNodeIntention) {
                locator.Fulfil(updateConnectedNodeIntention);
            }
        }

        public IEnumerable<Intention> ReceiveIntentions() {
            return locator.ReceiveIntentions();
        }


    }

}

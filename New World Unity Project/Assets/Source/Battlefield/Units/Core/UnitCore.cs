using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units.Core {

    public class UnitCore : IIntending {

        // Fields.

        // Account.
        private readonly UnitAccount account;

        // Modules.
        private readonly UnitLocator locator;
        private readonly float size;

        // TODO. Make another giving orders class.
        private bool plannedMotion = false;
        private float nextMovementTime = 0;


        // Properties.

        public Vector2Int ConnectedNode => locator.ConnectedNode;
        public float Size => size;


        // Interactions.

        public Vector3 GetPosition() {
            return locator.GetPosition();
        }


        // Constructor.

        public UnitCore(UnitDescription description) {
            if (description == null) {
                throw new System.ArgumentNullException(nameof(description));
            }
            account = new UnitAccount(this);
            locator = new UnitLocator(account, new Abilities.SimpleMotion(account), description.ConnectedNode);
            size = description.Size;
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
            Walk();
            return locator.ReceiveIntentions();
        }


        // Support.

        private void Walk() {
            if (!locator.CanMove || locator.Moving) {
                return;
            }
            if (!plannedMotion) {
                plannedMotion = true;
                nextMovementTime = Time.time + Random.Range(1f, 2f);
            }
            if (plannedMotion) {
                if (Time.time >= nextMovementTime) {
                    plannedMotion = false;
                    Vector2Int offset = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
                    locator.StartMotion(locator.ConnectedNode + offset);
                }
            }
        }


    }

}

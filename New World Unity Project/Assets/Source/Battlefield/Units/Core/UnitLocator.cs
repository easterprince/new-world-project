using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Battlefield.Units.Abilities;
using NewWorld.Battlefield.Units.Intentions;
using System.Collections.Generic;

namespace NewWorld.Battlefield.Units.Core {

    class UnitLocator : UnitCoreModule {

        // Fields.

        private Vector2Int connectedNode;
        private Vector3 currentPosition;

        private readonly MotionAbility motionAbility;


        // Properties.

        public Vector2Int ConnectedNode => connectedNode;
        public bool CanMove => motionAbility != null;
        public bool Moving => motionAbility?.StartedMotion ?? false;


        // Constructor.

        public UnitLocator(UnitAccount unitAccount, MotionAbility motionAbility, Vector2Int connectedNode) : base(unitAccount) {
            this.motionAbility = motionAbility;
            this.connectedNode = connectedNode;
        }


        // Interaction.

        public Vector3 GetPosition() {
            if (motionAbility != null && motionAbility.StartedMotion) {
                currentPosition = motionAbility.GetPositionInMotion();
            } else {
                float z = MapController.Instance.GetSurfaceHeight(connectedNode);
                currentPosition = new Vector3(connectedNode.x, connectedNode.y, z);
            }
            return currentPosition;
        }

        public void StartMotion(Vector2Int targetedNode) {
            if (motionAbility == null) {
                throw new System.InvalidOperationException("No motion ability - motion is impossible!");
            }
            motionAbility.StartMotion(connectedNode, targetedNode);
        }

        public void StopMotion() {
            if (motionAbility == null) {
                return;
            }
            motionAbility.StopMotion();
        }


        // Intentions.

        public override IEnumerable<Intention> ReceiveIntentions() {
            if (motionAbility?.StartedMotion ?? false) {
                return motionAbility.ReceiveIntentions();
            }
            return null;
        }

        public override void Fulfil(Intention intention) {
            if (intention == null) {
                throw new System.ArgumentNullException(nameof(intention));
            }
            if (intention is UpdateConnectedNodeIntention updateConnectedNodeIntention) {
                connectedNode = updateConnectedNodeIntention.NewConnectedNode;
                if (motionAbility != null) {
                    if (connectedNode != motionAbility.TargetedNode) {
                        motionAbility.StopMotion();
                    }
                }
            }
        }


    }

}

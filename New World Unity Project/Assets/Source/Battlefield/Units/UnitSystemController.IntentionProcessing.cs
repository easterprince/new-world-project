using System.Collections.Generic;
using UnityEngine;
using NewWorld.Battlefield.Map;
using NewWorld.Utilities.Singletones;
using NewWorld.Battlefield.Units.Intentions;

namespace NewWorld.Battlefield.Units {

    public partial class UnitSystemController : SceneSingleton<UnitSystemController> {

        // Fields.

//        private List<MovingIntention> movingIntentions;


        // Initialize.

        private void InitializeIntentionProcessing() {
//            movingIntentions = new List<MovingIntention>();
        }


        // Actions collecting and applying.

        private void ProcessIntentions() {
            foreach (UnitController unit in units) {
                IEnumerable<Intention> intentions = unit.ReceiveIntentions();
                if (intentions == null) {
                    continue;
                }
                foreach (Intention intention in intentions) {
                    if (intention is UpdateConnectedNodeIntention updateConnectedNode) {
                        NodeDescription node = MapController.Instance.GetSurfaceNode(updateConnectedNode.NewConnectedNode);
                        if (node == null) {
                            continue;
                        }
                        unit.Fulfil(intention);
                    }
                }
            }
        }

    }

}
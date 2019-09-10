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
                List<Intention> intentions = unit.ReceiveIntentions();
                if (intentions == null) {
                    continue;
                }
                foreach (Intention intention in intentions) {
                    if (intention is ChangingConnectedNodeIntention changeConnectedNode) {
                        NodeDescription node = MapController.Instance.GetSurfaceNode(changeConnectedNode.NewConnectedNode);
                        if (node == null) {
                            continue;
                        }
                        unit.ChangeConnectedNode(changeConnectedNode);
                    }
                }
            }
        }

    }

}
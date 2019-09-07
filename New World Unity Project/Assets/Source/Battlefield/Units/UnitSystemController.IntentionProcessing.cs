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
                MovingIntention movingIntention = unit.ReceiveMovingIntention();
                if (movingIntention == null) {
                    continue;
                }
                NodeDescription node = MapController.Instance.GetSurfaceNode(movingIntention.Destination);
                if (node == null) {
                    continue;
                }
                unit.ChangeCurrentNode(movingIntention.Destination);
            }
        }

    }

}
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Abilities.Motions {
    
    public class MotionUsageAction : AbilityUsageActionBase<MotionAbilityPresentation> {

        // Fields.

        private readonly Vector3 destination;


        // Constructor.

        public MotionUsageAction(MotionAbilityPresentation ability, Vector3 destination) : base(ability) {
            this.destination = destination;
        }


        // Properties.

        public Vector3 Destination => destination;


    }

}

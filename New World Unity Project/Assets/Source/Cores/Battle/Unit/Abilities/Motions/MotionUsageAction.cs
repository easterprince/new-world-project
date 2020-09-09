using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Abilities.Motions {

    public class MotionUsageAction : AbilityUsageActionBase<IMotionAbilityPresentation> {

        // Fields.

        private readonly Vector3 destination;


        // Constructor.

        public MotionUsageAction(IMotionAbilityPresentation ability, Vector3 destination) : base(ability) {
            this.destination = destination;
        }


        // Properties.

        public Vector3 Destination => destination;


    }

}

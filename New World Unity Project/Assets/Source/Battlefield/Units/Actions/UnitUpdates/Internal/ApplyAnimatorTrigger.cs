using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal {

    public class ApplyAnimatorTrigger : InternalUnitUpdate {

        // Fields.

        private readonly int animationTriggerHash;


        // Properties.

        public int AnimationTriggerHash => animationTriggerHash;


        // Constructor.

        public ApplyAnimatorTrigger(UnitController updatedUnit, int animationTriggerHash) : base(updatedUnit) {
            this.animationTriggerHash = animationTriggerHash;
        }


    }

}

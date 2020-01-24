using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates {

    public class AnimatorTriggerApplication : UnitUpdate {

        // Fields.

        private readonly int animationTriggerHash;


        // Properties.

        public int AnimationTriggerHash => animationTriggerHash;


        // Constructor.

        public AnimatorTriggerApplication(UnitController updatedUnit, int animationTriggerHash) : base(updatedUnit) {
            this.animationTriggerHash = animationTriggerHash;
        }


    }

}

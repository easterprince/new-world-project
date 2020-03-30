using NewWorld.Battlefield.Unit.Actions.UnitUpdates.Internal;
using UnityEngine;

namespace NewWorld.Battlefield.Unit {

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

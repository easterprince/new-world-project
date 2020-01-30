using UnityEngine;

namespace NewWorld.Battlefield.Units.Actions.UnitUpdates.Internal {

    public class AnimatorParameterUpdate<ParameterType> : InternalUpdate
        where ParameterType : struct {

        // Fields.

        private readonly int animationParameterHash;
        private readonly ParameterType newValue;


        // Properties.

        public int AnimationParameterHash => animationParameterHash;
        public ParameterType NewValue => newValue;


        // Constructor.

        public AnimatorParameterUpdate(UnitController updatedUnit, int animationParameterHash, ParameterType newValue) : base(updatedUnit) {
            this.animationParameterHash = animationParameterHash;
            this.newValue = newValue;
        }


    }

}

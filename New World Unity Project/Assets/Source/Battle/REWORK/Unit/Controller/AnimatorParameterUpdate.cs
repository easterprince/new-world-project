using UnityEngine;

namespace NewWorld.Battlefield.Unit {

    public class AnimatorParameterUpdate<ParameterType> : UnitUpdate
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

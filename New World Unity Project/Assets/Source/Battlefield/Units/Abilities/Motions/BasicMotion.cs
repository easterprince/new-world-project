using UnityEngine;
using NewWorld.Battlefield.Units.Conditions.Motions;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Abilities.Motions {

    public class BasicMotion : MotionAbility {

        // Constructor.

        public BasicMotion(float speed = 1) : base(speed) {}


        // Methods.

        sealed override public Condition Use(object parameterSet) {
            if (!(parameterSet is Vector2 destination)) {
                throw new System.ArgumentException($"Parameter set must be of type {typeof(Vector2)}.");
            }
            var condition = new DirectMotion(destination, Speed);
            return condition;
        }


    }

}
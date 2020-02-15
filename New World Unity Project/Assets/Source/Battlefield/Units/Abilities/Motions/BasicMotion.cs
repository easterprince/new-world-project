using UnityEngine;
using NewWorld.Battlefield.Units.Conditions.Motions;
using NewWorld.Battlefield.Units.Conditions;

namespace NewWorld.Battlefield.Units.Abilities.Motions {

    public class BasicMotion : MotionAbility {

        // Constructor.

        public BasicMotion(UnitController owner, float speed = 1) : base(owner, speed) {}


        // Methods.

        sealed override public Condition Use(object parameterSet) {
            if (!(parameterSet is Vector2 destination)) {
                throw new System.ArgumentException($"Parameter set must be of type {typeof(Vector2)}.");
            }
            var condition = new DirectMotion(Owner, destination, Speed);
            return condition;
        }


    }

}
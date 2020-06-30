﻿using UnityEngine;
using NewWorld.Battlefield.Unit.Conditions.Motions;
using NewWorld.Battlefield.Unit.Conditions;

namespace NewWorld.Battlefield.Unit.Abilities.Motions {

    public class BasicMotion : MotionAbility {

        // Constructor.

        public BasicMotion(float speed = 1) : base(speed) {}


        // Methods.

        sealed override protected private UnitCondition MakeCondition(object parameterSet) {
            if (!(parameterSet is Vector2 destination)) {
                throw new System.ArgumentException($"Parameter set must be of type {typeof(Vector2)}.");
            }
            var condition = new DirectMotion(destination, Speed);
            return condition;
        }


    }

}
using NewWorld.Battle.Cores.Unit.Body;
using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Conditions.Motions;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Abilities.Motions {

    public class DirectMotionAbility : MotionAbility {

        // Fields.

        private float speed;


        // Constructor.

        public DirectMotionAbility(float speed = 1f) {
            Speed = speed;
        }

        public DirectMotionAbility(DirectMotionAbility other) {
            speed = other.speed;
        }


        // Properties.

        public float Speed {
            get => speed;
            set => speed = Mathf.Max(value, 0);
        }

        public override float MovementPerSecond => speed;

        public override string Name => "Direct motion";
        public override string Description => "Move to target position.";


        // Cloning.

        public override MotionAbility Clone() {
            return new DirectMotionAbility(this);
        }


        // Usage.

        public override bool CheckIfUsable(Vector3 destination) {
            ValidateContext();
            return true;
        }

        public override void Use(Vector3 destination) {
            ValidateContext();
            var condition = new DirectMotionCondition(destination, speed);
            Owner.PlanAction(new ConditionChangingAction(condition, forceChange: false));
        }


    }

}

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

        public override string Description => "Move to target position.";


        // Cloning.

        public override MotionAbility Clone() {
            return new DirectMotionAbility(this);
        }


        // Usage.

        public override bool CheckIfUsable(Vector3 target) {
            ValidateContext();
            throw new NotImplementedException();
        }

        public override void Use(Vector3 target) {
            ValidateContext();
            throw new NotImplementedException();
        }


    }

}

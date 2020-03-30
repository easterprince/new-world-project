using UnityEngine;

namespace NewWorld.Battlefield.Unit.Abilities.Motions {

    public abstract class MotionAbility : UnitAbility {

        // Static.

        public static object FormParameterSet(Vector2 destination) {
            return destination;
        }


        // Fields.

        private readonly float speed;


        // Properties.

        public float Speed => speed;
        override public string Name => "Move";


        // Constructor.

        public MotionAbility(float speed = 1) : base() {
            this.speed = Mathf.Max(speed, 0f);
        }


    }

}
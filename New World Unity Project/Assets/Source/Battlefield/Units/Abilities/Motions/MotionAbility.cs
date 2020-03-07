using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Motions {

    public abstract class MotionAbility : Ability<MotionAbilityPresentation> {

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
            Presentation = new MotionAbilityPresentation(this);
        }


    }

}
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

        override public MotionAbilityPresentation Presentation => new MotionAbilityPresentation(this);

        public float Speed => speed;


        // Constructor.

        public MotionAbility(UnitController owner, float speed = 1) : base(owner) {
            this.speed = Mathf.Max(speed, 0f);
        }


    }

}
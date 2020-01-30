using UnityEngine;

namespace NewWorld.Battlefield.Units.Abilities.Motions {

    public abstract class MotionAbility : Ability {

        // Static.

        public static object FormParameterSet(Vector2Int destination) {
            return destination;
        }


        // Fields.

        private readonly float speed;


        // Properties.

        public float Speed => speed;


        // Constructor.

        public MotionAbility(UnitController owner, float speed = 1) : base(owner) {
            this.speed = Mathf.Max(speed, 0f);
        }


    }

}
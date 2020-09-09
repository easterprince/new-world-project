using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Cores.Battle.Unit.Conditions.Motions;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Abilities.Motions {

    public class DirectMotionAbility : AbilityModuleBase<IMotionAbility, IMotionAbilityPresentation>, IMotionAbility {

        // Fields.

        // Meta.
        private readonly NamedId id;

        // Motion properties.
        private readonly float speed;


        // Constructor.

        public DirectMotionAbility(NamedId id, float speed) {
            this.id = id;
            this.speed = Floats.SetPositive(speed);
        }

        public DirectMotionAbility(DirectMotionAbility other) {
            id = other.id;
            speed = other.speed;
        }


        // Properties.

        public float MovementPerSecond => speed;

        public override string Name => "Direct motion";
        public override string Description => "Move to target position.";


        // Cloning.

        public override IMotionAbility Clone() {
            return new DirectMotionAbility(this);
        }


        // Presentation generation.

        private protected override IMotionAbilityPresentation BuildPresentation() {
            return new MotionAbilityPresentation(this);
        }


        // Usage.

        public bool CheckIfUsable(Vector3 destination) {
            ValidateContext();
            return true;
        }

        public void Use(Vector3 destination) {
            ValidateContext();
            var condition = new DirectMotionCondition(destination, speed, id);
            Owner.PlanAction(new ConditionChangingAction(condition, forceChange: false));
        }


    }

}

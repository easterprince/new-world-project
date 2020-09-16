using NewWorld.Cores.Battle.Unit.Conditions;
using NewWorld.Cores.Battle.Unit.Conditions.Motions;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Cores.Battle.Unit.Abilities.Motions {

    public class DirectMotionAbility : AbilityModuleBase<IMotionAbility, IMotionAbilityPresentation>, IMotionAbility {

        // Fields.

        // Meta.
        private readonly NamedId conditionId;

        // Motion properties.
        private readonly float speed;


        // Constructor.

        public DirectMotionAbility(NamedId abilityId, NamedId conditionId, float speed) : base(abilityId) {
            this.conditionId = conditionId;
            this.speed = Floats.SetPositive(speed);
        }

        private DirectMotionAbility(DirectMotionAbility other) : base(other) {
            conditionId = other.conditionId;
            speed = other.speed;
        }


        // Properties.

        public float MovementPerSecond => speed;


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
            var condition = new DirectMotionCondition(destination, speed, conditionId);
            Owner.PlanAction(new ConditionChangingAction(condition, forceChange: false));
        }


    }

}

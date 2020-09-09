using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Conditions.Attacks;
using NewWorld.Battle.Cores.Unit.Durability;
using NewWorld.Utilities;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {

    public class DirectAttackAbility :
        AbilityModuleBase<IAttackAbility, IAttackAbilityPresentation>, IAttackAbility {

        // Fields.

        // Meta.
        private NamedId conditionId;

        // Attack properties.
        private readonly Damage singleAttackDamage;
        private readonly float attackDuration;
        private readonly float attackMoment;
        private readonly float attackRange;


        // Constructors.

        public DirectAttackAbility(
            Damage singleAttackDamage,
            float attackDuration, float attackMoment, float attackRange,
            NamedId conditionId) {

            // Meta.
            this.conditionId = conditionId;

            //Attack properties.
            this.singleAttackDamage = singleAttackDamage;
            this.attackDuration = Floats.SetPositive(attackDuration);
            this.attackMoment = Floats.LimitPositive(attackMoment, this.attackDuration);
            this.attackRange = Floats.SetPositive(attackRange);

        }

        public DirectAttackAbility(DirectAttackAbility other) {

            // Meta.
            conditionId = other.conditionId;

            // Attack properties.
            singleAttackDamage = other.singleAttackDamage;
            attackDuration = other.attackDuration;
            attackMoment = other.attackMoment;
            attackRange = other.attackRange;

        }


        // Properties.

        public Damage SingleAttackDamage => singleAttackDamage;
        public float AttackDuration => attackDuration;
        public float AttackMoment => attackMoment;
        public float AttackRange => attackRange;

        public Damage DamagePerSecond {
            get {
                if (singleAttackDamage.IsZero || float.IsPositiveInfinity(attackMoment)) {
                    return Damage.Zero;
                }
                if (float.IsPositiveInfinity(attackDuration)) {
                    return (singleAttackDamage / attackMoment);
                }
                return singleAttackDamage / attackDuration;
            }
        }

        public override string Name => "Direct attack";
        public override string Description => "Directly attack target.";


        // Cloning.

        public override IAttackAbility Clone() {
            return new DirectAttackAbility(this);
        }


        // Presentation.

        private protected override IAttackAbilityPresentation BuildPresentation() {
            return new AttackAbilityPresentation(this);
        }


        // Usage.

        public bool CheckIfUsable(UnitPresentation target) {
            if (target is null || Context is null || target.Context != Context) {
                return false;
            }
            Vector3 ownerPosition = Owner.Body.Position;
            Vector3 targetPosition = Owner.Body.Position;
            return (ownerPosition - targetPosition).magnitude <= attackRange;
        }

        public void Use(UnitPresentation target) {
            if (!CheckIfUsable(target)) {
                return;
            }
            var condition = new DirectAttackCondition(
                target: target,
                singleAttackDamage: singleAttackDamage,
                attackDuration: attackDuration,
                attackMoment: attackMoment,
                attackRange: attackRange,
                id: conditionId
            );
            Owner.PlanAction(new ConditionChangingAction(condition, forceChange: false));
        }


    }

}

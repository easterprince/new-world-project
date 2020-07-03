using NewWorld.Battle.Cores.Unit.Conditions;
using NewWorld.Battle.Cores.Unit.Conditions.Attacks;
using NewWorld.Battle.Cores.Unit.Durability;
using System;
using UnityEngine;

namespace NewWorld.Battle.Cores.Unit.Abilities.Attacks {

    public class DirectAttackAbility : AttackAbility {

        // Fields.

        private Damage singleAttackDamage;
        private float attackDuration;
        private float attackMoment;
        private float attackRange;


        // Constructors.

        public DirectAttackAbility() {
            singleAttackDamage = new Damage();
            attackDuration = 1f;
            attackMoment = 1f;
            attackRange = 1f;
        }

        public DirectAttackAbility(DirectAttackAbility other) {
            singleAttackDamage = other.singleAttackDamage;
            attackDuration = other.attackDuration;
            attackMoment = other.attackMoment;
            attackRange = other.attackRange;
        }


        // Properties.

        public Damage SingleAttackDamage {
            get => singleAttackDamage;
            set => singleAttackDamage = value;
        }

        public float AttackDuration {
            get => attackDuration;
            set {
                attackDuration = Mathf.Max(value, 0);
                AttackMoment = attackMoment;
            }
        }

        public float AttackMoment {
            get => attackMoment;
            set => attackMoment = Mathf.Clamp(value, 0, attackDuration);
        }

        public float AttackRange {
            get => attackRange;
            set => attackRange = Mathf.Max(value, 0);
        }

        public override Damage DamagePerSecond => singleAttackDamage / attackDuration;

        public override string Description => "Directly attack target.";


        // Cloning.

        public override AttackAbility Clone() {
            return new DirectAttackAbility(this);
        }


        // Udage.

        public override bool CheckIfUsable(UnitPresentation target) {
            if (target is null) {
                throw new ArgumentNullException(nameof(target));
            }
            ValidateContext();
            Vector3 ownerPosition = Owner.Body.Position;
            Vector3 targetPosition = Owner.Body.Position;
            return (ownerPosition - targetPosition).magnitude <= attackRange;
        }

        public override void Use(UnitPresentation target) {
            if (target is null) {
                throw new ArgumentNullException(nameof(target));
            }
            ValidateContext();
            if (CheckIfUsable(target)) {
                var condition = new DirectAttackCondition(target, singleAttackDamage, attackDuration, attackMoment, attackRange);
                Owner.PlanAction(new ConditionCausingAction(condition));
            }
        }


    }

}
